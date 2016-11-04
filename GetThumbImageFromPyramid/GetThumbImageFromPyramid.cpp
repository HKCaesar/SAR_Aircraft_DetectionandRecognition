// GetThumbImageFromPyramid.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include <stdio.h>
#include <iostream>
#include <fstream>
#include <string>
#include <stdlib.h>
#include <math.h>
#include <float.h>
#include <iomanip>
#include <time.h>
#include <algorithm>

#include "opencv/cv.h"
#include "opencv/cv.hpp"
#include "opencv2/opencv.hpp"
#include "opencv2/core/core.hpp"

using namespace cv;
using namespace std;

//函数声明和定义
#pragma region 图像增强函数
//函数：图像增强 
//输入：(1)单通道Mat图像矩阵，CV_32F;(2)增强系数：head，tail（取值范围：0.000~1.000）
//输出：(1)单通道Mat图像矩阵，CV_8U
Mat im_enhance(Mat img_input, double head, double tail)
{
	float imin, imax;
	Mat imgvec = img_input.clone();
	imgvec = imgvec.reshape(0, 1);
	cv::sort(imgvec, imgvec, CV_SORT_EVERY_ROW + CV_SORT_ASCENDING);
	int npix = countNonZero(imgvec);
	int nzero = img_input.rows*img_input.cols - npix;

	int idx1, idx2;
	if (head == 0)
	{
		imin = 0.0;
	}
	else
	{
		idx1 = floor(head*(double)npix + 0.5) + nzero;
		imin = imgvec.at<float>(0, idx1 - 1);
	}

	idx2 = floor(tail*(double)npix + 0.5) + nzero;
	imax = imgvec.at<float>(0, idx2 - 1);
	imgvec.release();

	Mat idx_min, idx_max, img_eh;
	//将小于最小值的设置为imin
	inRange(img_input, 0, imin, idx_min);
	img_input.setTo(Scalar::all(imin), idx_min);
	//将大于最大值的设置为imax
	inRange(img_input, imax, 1.1*imax, idx_max);
	img_input.setTo(Scalar::all(imax), idx_max);
	img_input = (img_input - imin) / (imax - imin);
	convertScaleAbs(img_input, img_eh, 255, 0);
	img_eh.convertTo(img_eh, CV_8UC1);
	return img_eh;
}
#pragma endregion

int main()
{
#pragma region 初始化读取图像,取单通道图像
	string name;
	string imagepath,pluginpath;
	ifstream f_img_path;
	f_img_path.open("..\\config\\image_path.txt");
	if (f_img_path.fail())
	{
		cout << "Error: cannot find image_path.txt!" << endl;
		return -1;
	}

	f_img_path >> name;
	f_img_path >> imagepath;
	f_img_path >> name;
	f_img_path >> pluginpath;
	f_img_path.close();

	Mat img_ori = imread(imagepath, -1);//-1代表读入和原始数据维度保持一致
	if (!img_ori.data)
	{
		printf("No data!--Exiting the program \n");
		return -1;
	}
	//取出单通道图像制作缩略图，这是为了兼容特殊数据格式的双极化SAR图像
	Size img_size = img_ori.size();
	Mat img_singlechannel(img_ori.size(), CV_MAKETYPE(img_ori.depth(), 1));;
	if (img_ori.channels() > 1)
	{
		vector<Mat> img_channels;
		split(img_ori, img_channels);
		img_singlechannel = img_channels.at(0).clone();
		img_channels.clear();
	}
	else
	{
		img_singlechannel = img_ori.clone();
	}
	img_ori.release();
#pragma endregion

#pragma region 对图像进行增强处理，并规整到8bit
	//需要记住如下列表，Mat::type()返回如下值
	//enum { CV_8U=0, CV_8S=1, CV_16U=2, CV_16S=3, CV_32S=4, CV_32F=5, CV_64F=6 };
	//以上是一个通道的，若是两个通道的则在对应单通道格式上加8，三通道则加16，四通道加24
	Mat img_Regularized = Mat(img_singlechannel.size(), CV_8UC1);
	img_singlechannel.convertTo(img_singlechannel, CV_32F);
	double head, tail;
	if (img_singlechannel.type() == 0)
	{
		head = 0.000;
		tail = 0.999;
		img_Regularized = im_enhance(img_singlechannel, head, tail);
	}
	else
	{
		double img_minVal, img_maxVal;
		minMaxLoc(img_singlechannel, &img_minVal, &img_maxVal, NULL, NULL);
		if (img_minVal < 0)
		{
			head = 0.008;
			tail = 0.995;
			Mat img_abovezero(img_singlechannel.size(), img_singlechannel.type());
			img_abovezero = img_singlechannel - img_minVal;
			//	subtract(img_singlechannel, *img_minVal, img_abovezero);
			img_Regularized = im_enhance(img_abovezero, head, tail);
		}
		else
		{
			head = 0.000;
			tail = 0.995;
			img_Regularized = im_enhance(img_singlechannel, head, tail);
		}

	}
	//输出是img_Regularized,代表已经图像增强的单通道8bit图像
#pragma endregion



#pragma region 判断图像大小是否需要制作缩略图
	int pyr_step; //图像金字塔层数
	if (img_Regularized.rows > 4096 || img_Regularized.cols > 4096)
	{
		float  MAX_sizeof2 = max(log2f((float)img_Regularized.rows), log2f((float)img_Regularized.cols));
		float  MIN_sizeof2 = min(log2f((float)img_Regularized.rows), log2f((float)img_Regularized.cols));
		pyr_step = floor(MAX_sizeof2 - log2(4096)) + 1;
		if (floor(MIN_sizeof2) - pyr_step < 7)
		{
			printf("Image size is inappropriate \n");
			return -1;
		}
	}
	else
	{
		imwrite("img_thumbnail.tiff", img_Regularized);
		return 0;
	}
#pragma endregion

#pragma region 	制作图像金字塔
	//输入：(1)变量名：img_Regularized,单通道Mat矩阵,CV_8U
	//输出：(1)变量名：img_Thumbnail,单通道Mat矩阵,CV_8U,高斯金字塔生成

	vector<Mat> img_pyramid;
	buildPyramid(img_Regularized, img_pyramid, pyr_step);
	Mat img_Thumbnail = img_pyramid[pyr_step].clone();
	imwrite("img_thumbnail.tiff", img_Thumbnail);
	img_Thumbnail.release();
	
	//vector类型数据的正确释放,clear+swap
	//详见《Effective STL》 17
	img_pyramid.clear();    
	vector<Mat>(img_pyramid).swap(img_pyramid);


#pragma endregion




	return 0;
}

