#include "stdafx.h"
#include "CoordinateTransform.h"

#pragma region ����ϵת�� һά����汾
//WKT�ı��� 6��ֵ����Ҫת���ĵ�������Ҫת�������ص�����
void coortransform(char* pszSrcWkt, double adfGeoTransform[6], int ncount, double* xpixel, double* ypixel, double* x, double* y)
{

	double *X;
	double *Y;
	X = new double[ncount]();
	Y = new double[ncount]();
	double* Z = { 0 };
	for (int i = 0; i<ncount; ++i) {
		X[i] = adfGeoTransform[0] + xpixel[i] * adfGeoTransform[1] + ypixel[i] * adfGeoTransform[2];
		Y[i] = adfGeoTransform[3] + xpixel[i] * adfGeoTransform[4] + ypixel[i] * adfGeoTransform[5];
	}//ƽ������


	 //����OGR�Ŀռ�ο�ϵ  
	OGRSpatialReference oSrcSrs;  //Դ����ϵͳ  
	OGRSpatialReference *oDestSrs;   //Ŀ������ϵͳ  
	OGRCoordinateTransformation *poTransform;

	oSrcSrs.importFromWkt(&pszSrcWkt);

	oDestSrs = oSrcSrs.CloneGeogCS();
	poTransform = OGRCreateCoordinateTransformation(&oSrcSrs, oDestSrs);
	/*if (poTransform==NULL)
	{
	return false;
	}  */
	int nFlag = poTransform->Transform(ncount, X, Y, NULL);
	for (int i = 0; i<ncount; ++i)x[i] = X[i];
	for (int j = 0; j<ncount; ++j)y[j] = Y[j];





	if (nFlag)
	{
		OGRCoordinateTransformation::DestroyCT(poTransform);

	}


	return;
}
#pragma endregion

#pragma region ����ϵת�� Point2f�汾
//WKT�ı��� 6��ֵ����Ҫת���ĵ�������Ҫת�������ص�����
void coortransformP2f(char* pszSrcWkt, double adfGeoTransform[6], int ncount, Point2f *pixel_XY, Point2f *pixel_LongLat)
{

	double *X;
	double *Y;
	X = new double[ncount]();
	Y = new double[ncount]();
	double *Z = { 0 };

	for (int i = 0; i<ncount; i++)
	{
		X[i] = adfGeoTransform[0] + (double)pixel_XY[i].x * adfGeoTransform[1] + (double)pixel_XY[i].x * adfGeoTransform[2];
		Y[i] = adfGeoTransform[3] + (double)pixel_XY[i].x * adfGeoTransform[4] + (double)pixel_XY[i].y * adfGeoTransform[5];
		//cout<<X[i]<<" "<<Y[i]<<endl;
	}//ƽ������


	 //����OGR�Ŀռ�ο�ϵ  
	OGRSpatialReference oSrcSrs;  //Դ����ϵͳ  
	OGRSpatialReference *oDestSrs;   //Ŀ������ϵͳ  
	OGRCoordinateTransformation *poTransform;
	char *npszSrcWkt = new char[strlen(pszSrcWkt) + 1];
	strcpy_s(npszSrcWkt, strlen(pszSrcWkt) + 1,pszSrcWkt);
	oSrcSrs.importFromWkt(&npszSrcWkt);
	oDestSrs = oSrcSrs.CloneGeogCS();
	poTransform = OGRCreateCoordinateTransformation(&oSrcSrs, oDestSrs);
	if (poTransform == NULL)
	{
		cout << "poTransform is False!" << endl;
		system("pause");
		return;
	}

	int nFlag = poTransform->Transform(ncount, X, Y, NULL);
	for (int i = 0; i<ncount; ++i)
	{
		pixel_LongLat[i].x = (float)X[i];
		pixel_LongLat[i].y = (float)Y[i];
	}


	if (nFlag)
	{
		OGRCoordinateTransformation::DestroyCT(poTransform);

	}


	return;
}
#pragma endregion
