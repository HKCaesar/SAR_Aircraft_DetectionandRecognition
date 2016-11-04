#include "stdafx.h"
#include "TD_global.h"
#include "SD_coortransform.h" 

int main( int argc, char** argv )
{
	string name, ImagePath, PluginfoldPath;
	
	ifstream infile;  
    infile.open("..\\config\\path.txt");      //注意文件的路径 
	
	if (infile.fail())
	{
		cout << "Error: cannot find path.txt!" << endl;
		return -1;
	}
	infile >> name;
	cout << name <<endl;
    infile>>ImagePath;
	cout << ImagePath <<endl;
	infile >> name;
	cout << name <<endl;
	infile>>PluginfoldPath;     
	cout << PluginfoldPath <<endl;
    infile.close();  

	ofstream outfile, outfile1;  
    outfile.open(PluginfoldPath + "\\config\\config.txt");
	outfile1.open(PluginfoldPath + "\\bin_readVIF\\ImageINFO.txt");
	if (outfile.fail())
	{
		cout << "Error: cannot find config.txt!" << endl;
		return -1;
	}
	if (outfile1.fail())
	{
		cout << "Error: cannot find ImageINFO.txt" << endl;
		return -1;
	}
	GDALDataset* poDataset;
    GDALAllRegister();
	CPLSetConfigOption("GDAL_FILENAME_IS_UTF8","NO");//读中文

	poDataset=(GDALDataset*)GDALOpen(ImagePath.c_str(),GA_ReadOnly);
	if( poDataset == NULL )/*检查是否正常打开文件*/
	{
		cout<<"Can not open the image file!"<<endl;
		exit(-1);
	}
	int nGCPs=poDataset->GetGCPCount();   //获得控制点数目
    cout<<"控制点数目:"<<nGCPs<<endl;

	//描述信息
	const char* imgdes=poDataset->GetDescription();
	cout<<"描述信息:"<<imgdes<<endl;
	string imagepath(imgdes);
	 string imagename ;
	size_t pos = imagepath.find_last_of('\\');
	size_t pos1 = imagepath.find_last_of('.');
	cout << pos << endl; 
	cout << pos1 << endl;
	
	if(pos != -1)
    {
		imagename = imagepath.substr(pos + 1,pos1-pos-1);
    }
   
	cout << imagename << endl;
	int  nImgSizeX=poDataset->GetRasterXSize(); //width图像宽度(以像素个数计)
	cout<<"图像宽度"<<nImgSizeX<<endl;
	
	int  nImgSizeY=poDataset->GetRasterYSize();//height图像高度(以像素个数计)
	cout<<"图像高度"<<nImgSizeY<<endl;

    const char* pszSrcWkt=poDataset->GetProjectionRef();
	/*const int number = strlen(pszSrcWkt);
	cout << number <<endl;*/
	char* preLpszSrcWkt =const_cast<char*>(pszSrcWkt);
	int ncount1=strlen(pszSrcWkt);
	cout<<ncount1<<endl;
	int verhicle_flag = 1;
	if (ncount1 == 0)
	{
		cout<<"图像没有投影信息"<<endl;
		verhicle_flag = 0;
	}
	cout << verhicle_flag <<endl;
	if(verhicle_flag == 0)
	{
	outfile << "<SARImagePath>" << endl;
	outfile << imgdes <<endl;

	outfile << "<RefImagePath>" << endl;
	outfile << PluginfoldPath + "\\result\\img_en.tif" <<endl;

    outfile << "<OutputDir>" << endl;
	outfile << PluginfoldPath + "\\result" <<endl;

	outfile << "<ImageName>" << endl;
	outfile << imagename <<endl;

	outfile << "<ResultImagePath>" << endl;
	outfile << "MRFresult.tif" <<endl;

	outfile << "<ResultXmlPath>" << endl;
	outfile << "result.xml" <<endl;

	outfile << "<MinLongtitude>" << endl;
	outfile <<  0 <<endl;

	outfile << "<MaxLongitude>" << endl;
	outfile << nImgSizeX <<endl;

	outfile << "<MinLatitude>" << endl;
	outfile << -nImgSizeY<<endl;

	outfile << "<MaxLatitude>" << endl;
	outfile << 0 <<endl;

	outfile << "<PixelSize>" << endl;
	outfile <<  1 << endl;

	outfile << "<ImageRows>" << endl;
	outfile << nImgSizeY <<endl;

	outfile << "<ImageCols>" << endl;
	outfile << nImgSizeX <<endl;

	outfile.close();

	//写ImageINFO.txt
	outfile1 << "<SARImagePath>" << endl;
	outfile1 << imgdes <<endl;

	outfile1 << "<RefImagePath>" << endl;
	outfile1<< PluginfoldPath + "\\result\\img_en.tif" <<endl;

    outfile1 << "<OutputDir>" << endl;
	outfile1 << PluginfoldPath + "\\result" <<endl;

	outfile1 << "<ImageName>" << endl;
	outfile1 << imagename <<endl;

	outfile1 << "<ImageHeight(Rows)>" << endl;
	outfile1 << nImgSizeY <<endl;

	outfile1 << "<ImageWidth(Cols)>" << endl;
	outfile1 << nImgSizeX <<endl;

	outfile1 << "<LatMin>" << endl;
	outfile1 << -nImgSizeY <<endl;

	outfile1 << "<LatMax>" << endl;
	outfile1 << 0 <<endl;

	outfile1 << "<LongMin>" << endl;
	outfile1 << 0 <<endl;

	outfile1 << "<LongMax>" << endl;
	outfile1 << nImgSizeX <<endl;

	outfile1 << "<PixelSize>" << endl;
	outfile1 << 1 <<endl;

	
	
	
	outfile1.close();
	return 0;
	}

	//地理坐标信息
	double adfGeoTransform[6];
	poDataset->GetGeoTransform( adfGeoTransform );
	for(int i = 0; i < 6; i++)
		cout<< setiosflags(ios::fixed) << setprecision(12) << adfGeoTransform[i]<<endl;

	
    
	double X[4];
	double Y[4];
	X[0] = 0;
	Y[0] = 0;//左上像素坐标
	X[1] = (float)nImgSizeX;
	Y[1] = 0;//右上
	X[2] = 0;
	Y[2] = (float)nImgSizeY;//左下
	X[3] = (float)nImgSizeX;
	Y[3] = (float)nImgSizeY;//右下
	double X_lon[4];
	double Y_lat[4];
	coortransform(preLpszSrcWkt,&adfGeoTransform[0],4,X,Y,X_lon,Y_lat);
	
	 for(int i=0; i<4; i++){
		cout << setiosflags(ios::fixed) << setprecision(13) <<"(" << X_lon[i] << " , " << Y_lat[i]<< ")" <<endl; 	
		}
	
	double MaxLon = X_lon[3] ;
    double MinLon = X_lon[0] ;
	double MaxLat = Y_lat[0] ;
	double MinLat = Y_lat[3] ;


	//写conifg.txt
	outfile << "<SARImagePath>" << endl;
	outfile << imgdes <<endl;

	outfile << "<RefImagePath>" << endl;
	outfile << PluginfoldPath + "\\result\\img_en.tif" <<endl;

    outfile << "<OutputDir>" << endl;
	outfile << PluginfoldPath + "\\result" <<endl;

	outfile << "<ImageName>" << endl;
	outfile << imagename <<endl;

	outfile << "<ResultImagePath>" << endl;
	outfile << "MRFresult.tif" <<endl;

	outfile << "<ResultXmlPath>" << endl;
	outfile << "result.xml" <<endl;

	outfile << "<MinLongtitude>" << endl;
	outfile << setiosflags(ios::fixed) << setprecision(13) << MinLon <<endl;

	outfile << "<MaxLongitude>" << endl;
	outfile << MaxLon <<endl;

	outfile << "<MinLatitude>" << endl;
	outfile << MinLat <<endl;

	outfile << "<MaxLatitude>" << endl;
	outfile << MaxLat <<endl;

	outfile << "<PixelSize>" << endl;
	outfile << adfGeoTransform[1] <<endl;

	outfile << "<ImageRows>" << endl;
	outfile << nImgSizeY <<endl;

	outfile << "<ImageCols>" << endl;
	outfile << nImgSizeX <<endl;

	outfile.close();

	//写ImageINFO.txt
	outfile1 << "<SARImagePath>" << endl;
	outfile1 << imgdes <<endl;

	outfile1 << "<RefImagePath>" << endl;
	outfile1<< PluginfoldPath + "\\result\\img_en.tif" <<endl;

    outfile1 << "<OutputDir>" << endl;
	outfile1 << PluginfoldPath + "\\result" <<endl;

	outfile1 << "<ImageName>" << endl;
	outfile1 << imagename <<endl;

	outfile1 << "<ImageHeight(Rows)>" << endl;
	outfile1 << nImgSizeY <<endl;

	outfile1 << "<ImageWidth(Cols)>" << endl;
	outfile1 << nImgSizeX <<endl;

	outfile1 << "<LatMin>" << endl;
	outfile1 << setiosflags(ios::fixed) << setprecision(13) << MinLat <<endl;

	outfile1 << "<LatMax>" << endl;
	outfile1 << MaxLat <<endl;

	outfile1 << "<LongMin>" << endl;
	outfile1 << MinLon <<endl;

	outfile1 << "<LongMax>" << endl;
	outfile1 << MaxLon <<endl;

	outfile1 << "<PixelSize>" << endl;
	outfile1 << adfGeoTransform[1] <<endl;

	
	
	
	outfile1.close();

	/*system("pause");
	waitKey();*/
	return 0;
}