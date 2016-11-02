// GetThumbImageFromPyramid.cpp : �������̨Ӧ�ó������ڵ㡣
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

#include "opencv\cv.h"
#include "opencv\cv.hpp"
#include "opencv2\opencv.hpp"

using namespace cv;
using namespace std;

int main()
{
	string name;
	string imagepath;
	ifstream f_img_path;
	f_img_path.open("..\\config\\image_path.txt");
	if (f_img_path.fail())
	{
		cout << "Error: cannot find image_path.txt!" << endl;
		return -1;
	}

	f_img_path >> name;
	f_img_path >> imagepath;
	f_img_path.close();

	Mat img_ori = imread(imagepath, -1);//-1��������ԭʼ����ά�ȱ���һ��
	if (!img_ori.data)
	{
		printf("No data!--Exiting the program \n");
		return -1;
	}
	//ȡ����ͨ��ͼ����������ͼ������Ϊ�˼����������ݸ�ʽ��˫����SARͼ��
	Size img_size = img_ori.size();
	Mat img_singlechannel(img_ori.size(), CV_MAKETYPE(img_ori.depth(), 1));;
	if (img_ori.channels() > 1)
	{
		vector<Mat> img_channels;
		split(img_ori, img_channels);
		img_singlechannel = img_channels.at(0).clone();
		img_channels.erase;
	}
	else
	{
		img_singlechannel = img_ori.clone();
	}
	img_ori.release();
	//��ͼ�������ǿ������������8bit
	//��Ҫ��ס�����б�Mat::type()��������ֵ
	//enum { CV_8U=0, CV_8S=1, CV_16U=2, CV_16S=3, CV_32S=4, CV_32F=5, CV_64F=6 };
	//������һ��ͨ���ģ���������ͨ�������ڶ�Ӧ��ͨ����ʽ�ϼ�8����ͨ�����16����ͨ����24
	Mat img_Regularized = Mat(img_singlechannel.size(), CV_8UC1);
	img_singlechannel.convertTo(img_singlechannel, CV_32F);
	double head,tail; 
	if (img_singlechannel.type() == 0)
	{
		head = 0.000;
		tail = 0.999;
		img_Regularized = im_enhance(img_singlechannel, head, tail);
	}
	else
	{
		double* img_minVal,* img_maxVal;
		minMaxLoc(img_singlechannel, img_minVal, img_maxVal, NULL, NULL);
		if (*img_minVal < 0)
		{
			head = 0.008;
			tail = 0.995;
			Mat img_abovezero(img_singlechannel.size(), img_singlechannel.type());
			subtract(img_singlechannel, *img_minVal, img_abovezero);
			img_Regularized = im_enhance(img_abovezero, head, tail);
		}
		else
		{
			head = 0.000;
			tail = 0.995;
			img_Regularized = im_enhance(img_singlechannel, head, tail);
		}
		
	}
	img_singlechannel.convertTo(img_singlechannel, CV_32F, 1.0, 0.0);
	Mat img_singlechannel_copy = img_singlechannel.clone();

	
	//�ж�ͼ���С�Ƿ���Ҫ��������ͼ
	int pyr_step; //ͼ�����������
	if (img_singlechannel.rows > 4096 || img_singlechannel.cols > 4096)
	{
		float  MAX_sizeof2 = max(log2f((float)img_singlechannel.rows), log2f((float)img_singlechannel.cols));
		float  MIN_sizeof2 = min(log2f((float)img_singlechannel.rows), log2f((float)img_singlechannel.cols));
		pyr_step = floor(MAX_sizeof2 - log2(4096))+1;
		if (floor(MIN_sizeof2) - pyr_step < 7)
		{
			printf("Image size is inappropriate \n");
			return -1;
		}
	}
	else
	{
		imwrite("img_thumbnail.tiff", img_singlechannel);
		return 0;
	}
	//����ͼ�������
	img_singlechannel;





    return 0;
}

Mat im_enhance(Mat img_input,double head,double tail)
{
	float imin, imax;
	Mat imgvec = img_input.clone();
	imgvec = imgvec.reshape(0, 1);
	sort(imgvec, imgvec, CV_SORT_EVERY_ROW + CV_SORT_ASCENDING);
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

	Mat idx_min,idx_max,img_eh;
	//��С����Сֵ������Ϊimin
	inRange(img_input, 0, imin, idx_min);
	img_input.setTo(Scalar::all(imin), idx_min);
	//���������ֵ������Ϊimax
	inRange(img_input, imax, 1.1*imax, idx_max);
	img_input.setTo(Scalar::all(imax), idx_max);
	img_input = (img_input - imin) / (imax - imin);
	convertScaleAbs(img_input, img_eh, 255, 0);
	img_eh.convertTo(img_eh, CV_8UC1);
	return img_eh;
}