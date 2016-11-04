
#ifndef COORTRANSFORM_H
#define COORTRANSFORM_H


#include "Global.h"



//像素坐标转换为经纬度坐标---一维数组版本
void coortransform(char* pszSrcWkt, double adfGeoTransform[6], int ncount, double* xpixel, double* ypixel, double* x, double* y);
//像素坐标转换为经纬度坐标---Point2f版本
void coortransformP2f(char* pszSrcWkt, double adfGeoTransform[6], int ncount, Point2f *pixel_XY, Point2f *pixel_LongLat);
#endif