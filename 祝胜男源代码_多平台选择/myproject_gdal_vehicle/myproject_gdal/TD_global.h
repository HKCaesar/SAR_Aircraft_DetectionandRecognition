
#ifndef TD_GLOBAL_H
#define TD_GLOBAL_H

#include "opencv/cv.h"
#include "opencv2/opencv.hpp"
#include <iostream>
#include <fstream>   
#include <string>
#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <time.h>
#include <iomanip>
#include <omp.h>
//gdal
#include"gdal/include/gdal_priv.h"
#include"gdal/include/ogrsf_frmts.h"
//proj





// 使用的是Release版本的lib
///

#pragma comment(lib,"..\\x64\\Release\\dll\\gdal_i.lib")
#pragma comment(lib,"..\\x64\\Release\\dll\\gdal_i.lib")
#pragma comment(lib,"..\\x64\\Release\\dll\\opencv_calib3d231.lib")
#pragma comment(lib,"..\\x64\\Release\\dll\\opencv_contrib231.lib")
#pragma comment(lib,"..\\x64\\Release\\dll\\opencv_core231.lib")
#pragma comment(lib,"..\\x64\\Release\\dll\\opencv_features2d231.lib")
#pragma comment(lib,"..\\x64\\Release\\dll\\opencv_flann231.lib")
#pragma comment(lib,"..\\x64\\Release\\dll\\opencv_gpu231.lib")
#pragma comment(lib,"..\\x64\\Release\\dll\\opencv_haartraining_engine.lib")
#pragma comment(lib,"..\\x64\\Release\\dll\\opencv_highgui231.lib")
#pragma comment(lib,"..\\x64\\Release\\dll\\opencv_imgproc231.lib")
#pragma comment(lib,"..\\x64\\Release\\dll\\opencv_legacy231.lib")
#pragma comment(lib,"..\\x64\\Release\\dll\\opencv_ml231.lib")
#pragma comment(lib,"..\\x64\\Release\\dll\\opencv_objdetect231.lib")
#pragma comment(lib,"..\\x64\\Release\\dll\\opencv_ts231.lib")
#pragma comment(lib,"..\\x64\\Release\\dll\\opencv_video231.lib")
#pragma comment(lib,"..\\x64\\Release\\dll\\pthreadVC2.lib")

using namespace std;
using namespace cv;


#endif