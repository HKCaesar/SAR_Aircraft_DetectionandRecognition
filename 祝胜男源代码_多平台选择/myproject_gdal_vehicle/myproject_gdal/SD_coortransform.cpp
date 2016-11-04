#include "stdafx.h"
#include "SD_coortransform.h"
#include "TD_global.h"

#pragma region 坐标系转换 一维数组版本
void coortransform(char* pszSrcWkt,double adfGeoTransform[6],int ncount,double* xpixel,double* ypixel,double* x,double* y)//WKT文本， 6个值，需要转换的点数，需要转换的像素点坐标
{
	  
       double *X;
	   double *Y;
	   X = new double[ncount]();
       Y = new double[ncount]();
       double* Z = {0};
	   for(int i=0;i<ncount;++i){
	X[i] = adfGeoTransform[0] + xpixel[i]*adfGeoTransform[1] + ypixel[i]*adfGeoTransform[2];
	Y[i] = adfGeoTransform[3] + xpixel[i]*adfGeoTransform[4] + ypixel[i]*adfGeoTransform[5];
	   }//平面坐标
	 
	
	  //创建OGR的空间参考系  
    OGRSpatialReference oSrcSrs;  //源坐标系统  
	OGRSpatialReference *oDestSrs;   //目的坐标系统  
	OGRCoordinateTransformation *poTransform;
	
    oSrcSrs.importFromWkt(&pszSrcWkt); 
    
    oDestSrs = oSrcSrs.CloneGeogCS(); 
    poTransform = OGRCreateCoordinateTransformation( &oSrcSrs, oDestSrs );
	/*if (poTransform==NULL) 
        {  
            return false;  
        }  */
	int nFlag =poTransform->Transform(ncount,X,Y,NULL);
	for(int i=0;i<ncount;++i)x[i]=X[i];
	for(int j=0;j<ncount;++j)y[j]=Y[j];
	

	

		 
	if (nFlag)  
        {  
            OGRCoordinateTransformation::DestroyCT(poTransform);  
         
		}
	

		return  ;
}
#pragma endregion

#pragma region 坐标系转换 Point2f版本

void coortransformP2f(char* pszSrcWkt,double adfGeoTransform[6],int ncount,Point2f *pixel_XY,Point2f *pixel_LongLat)
//WKT文本， 6个值，需要转换的点数，需要转换的像素点坐标
{
	  
       double *X;
	   double *Y;
	   X = new double[ncount]();
       Y = new double[ncount]();
	   double *Z = {0};

	   for(int i=0;i<ncount;i++)
	   {
		    X[i] = adfGeoTransform[0] + (double)pixel_XY[i].x * adfGeoTransform[1] + (double)pixel_XY[i].x * adfGeoTransform[2];
			Y[i] = adfGeoTransform[3] + (double)pixel_XY[i].x * adfGeoTransform[4] +  (double)pixel_XY[i].y * adfGeoTransform[5];
			//cout<<X[i]<<" "<<Y[i]<<endl;
	   }//平面坐标

	
	  //创建OGR的空间参考系  
    OGRSpatialReference oSrcSrs;  //源坐标系统  
	OGRSpatialReference *oDestSrs;   //目的坐标系统  
	OGRCoordinateTransformation *poTransform;
    char *npszSrcWkt = new char[strlen(pszSrcWkt)+1];
    strcpy(npszSrcWkt,pszSrcWkt);
    oSrcSrs.importFromWkt(&npszSrcWkt); 
    oDestSrs = oSrcSrs.CloneGeogCS(); 
    poTransform = OGRCreateCoordinateTransformation( &oSrcSrs, oDestSrs );
	if (poTransform==NULL) 
        {  
			cout<<"poTransform is False!"<<endl;
			system("pause");
            return ;  
        }  

	int nFlag =poTransform->Transform(ncount,X,Y,NULL);
	for(int i=0;i<ncount;++i)
		{
			pixel_LongLat[i].x = (float)X[i];
			pixel_LongLat[i].y = (float)Y[i];
	}
	

	

		 
	if (nFlag)  
        {  
            OGRCoordinateTransformation::DestroyCT(poTransform);  
         
		}
	

		return ;
}
#pragma endregion