
#ifndef COORTRANSFORM_H
#define COORTRANSFORM_H


#include "Global.h"



//��������ת��Ϊ��γ������---һά����汾
void coortransform(char* pszSrcWkt, double adfGeoTransform[6], int ncount, double* xpixel, double* ypixel, double* x, double* y);
//��������ת��Ϊ��γ������---Point2f�汾
void coortransformP2f(char* pszSrcWkt, double adfGeoTransform[6], int ncount, Point2f *pixel_XY, Point2f *pixel_LongLat);
#endif