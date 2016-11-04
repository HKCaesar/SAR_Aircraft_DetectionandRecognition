// GDAL_ReadImageInfo.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include <Windows.h>
#include "CoordinateTransform.h"

//将wchar转换为UTF-8格式
std::string wchar_to_utf8(const wchar_t* buffer, int len)
{
	int nChars = ::WideCharToMultiByte(
		CP_UTF8,
		0,
		buffer,
		len,
		NULL,
		0,
		NULL,
		NULL);
	if (nChars == 0)return"";

	string newbuffer;
	newbuffer.resize(nChars);
	::WideCharToMultiByte(
		CP_UTF8,
		0,
		buffer,
		len,
		const_cast<char*>(newbuffer.c_str()),
		nChars,
		NULL,
		NULL);

	return newbuffer;
}
//将wstring转换为UTF-8格式
std::string wstring_to_utf8(const std::wstring& str)
{
	return wchar_to_utf8(str.c_str(), (int)str.size());
}
//将string转换为UTF-8格式
std::string string_To_UTF8(const std::string & str)
{
	int nwLen = ::MultiByteToWideChar(CP_ACP, 0, str.c_str(), -1, NULL, 0);

	wchar_t * pwBuf = new wchar_t[nwLen + 1];//一定要加1，不然会出现尾巴
	ZeroMemory(pwBuf, nwLen * 2 + 2);

	::MultiByteToWideChar(CP_ACP, 0, str.c_str(), str.length(), pwBuf, nwLen);

	int nLen = ::WideCharToMultiByte(CP_UTF8, 0, pwBuf, -1, NULL, NULL, NULL, NULL);

	char * pBuf = new char[nLen + 1];
	ZeroMemory(pBuf, nLen + 1);

	::WideCharToMultiByte(CP_UTF8, 0, pwBuf, nwLen, pBuf, nLen, NULL, NULL);

	std::string retStr(pBuf);

	delete[]pwBuf;
	delete[]pBuf;

	pwBuf = NULL;
	pBuf = NULL;

	return retStr;
}

int main()
{
#pragma region 从/config/image_path.txt中获取图像数据地址
	string name, ImagePath, PluginfoldPath;

	ifstream infile;
	infile.open("..\\config\\image_path.txt");      //注意文件的路径 

	if (infile.fail())
	{
		cout << "Error: cannot find image_path.txt!" << endl;
		return -1;
	}
	infile >> name;
	cout << name << endl;
	infile >> ImagePath;
	cout << ImagePath << endl;
	infile >> name;
	cout << name << endl;
	infile >> PluginfoldPath;
	cout << PluginfoldPath << endl;
	infile.close();
#pragma endregion

#pragma region 从geotiff中读取地理编码信息，并写入/config/config.txt与ImageINFO.txt中
	//图像的地理编码目前接触过的有三种
	//(1)无地理编码
	//(2)标准坐标系的地理编码
	//(3)非标准的地理编码,以IECAS样图为例
	
	//1.声明文件流，创建两个txt文件
	ofstream outfile, outfile1;
	outfile.open(PluginfoldPath + "\\config\\config.txt");
	outfile1.open(PluginfoldPath + "\\bin_readinfo\\ImageINFO.txt");
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
	//2.利用GDAL库读取地理编码信息
	GDALDataset* poDataset;
	GDALAllRegister();
	CPLSetConfigOption("GDAL_FILENAME_IS_UTF8", "NO");//使GDAL可以读中文名称文件

	poDataset = (GDALDataset*)GDALOpen(ImagePath.c_str(), GA_ReadOnly);
	if (poDataset == NULL)/*检查是否正常打开文件*/
	{
		cout << "Can not open the image file!" << endl;
		exit(-1);
	}
	int nGCPs = poDataset->GetGCPCount();   //获得控制点数目
	cout << "控制点数目:" << nGCPs << endl;

	//描述信息
	const char* imgdes = poDataset->GetDescription();
	cout << "描述信息:" << imgdes << endl;
	string imagepath(imgdes);
	string imagename;
	size_t pos = imagepath.find_last_of('\\');
	size_t pos1 = imagepath.find_last_of('.');
	//cout << pos << endl;
	//cout << pos1 << endl;
	if (pos != -1)
	{
		imagename = imagepath.substr(pos + 1, pos1 - pos - 1);
	}
	cout << "图像名称: "<<imagename << endl;
	int  nImgSizeX = poDataset->GetRasterXSize(); //width图像宽度(以像素个数计)
	cout << "图像宽度: " << nImgSizeX << endl;
	int  nImgSizeY = poDataset->GetRasterYSize();//height图像高度(以像素个数计)
	cout << "图像高度: " << nImgSizeY << endl;

	const char* pszSrcWkt = poDataset->GetProjectionRef();
	char* preLpszSrcWkt = const_cast<char*>(pszSrcWkt);
	int ncount1 = strlen(pszSrcWkt);
	//cout << ncount1 << endl;
	int verhicle_flag = 1;
	if (ncount1 == 0)
	{
		cout << "图像没有投影信息" << endl;
		verhicle_flag = 0;
	}
	string TransferUTF8;
#pragma region 2.1 地理编码中无投影信息
	if (verhicle_flag == 0)
	{
		//写入config.txt
		outfile << "<SARImagePath>" << endl;
		outfile << imgdes << endl;
		outfile << "<SARImageName>" << endl;
		outfile << imagename << endl;
		outfile << "<MaskPath>" << endl;
		outfile << PluginfoldPath + "\\SAR_Result\\"+ imagename +"\\SAR_maskimg.tif" << endl;
		outfile << "<RefImagePath>" << endl;
		outfile << PluginfoldPath + "\\SAR_Result\\" + imagename + "\\img_en.tif" << endl;
		outfile << "<TemplatePath>" << endl;
		outfile << PluginfoldPath + "\\Template\\" << endl;
		outfile << "<ResultImagePath>" << endl;
		outfile << PluginfoldPath + "\\SAR_Result\\" + imagename + "\\"  << endl;
		outfile << "<ResultXmlPath>" << endl;
		outfile << PluginfoldPath + "\\SAR_Result\\" + imagename + "\\"+ imagename + "_SAR.xml" << endl;
		outfile << "<SARFusionXmlPath>" << endl;
		outfile << PluginfoldPath + "\\SAR_Result\\" + imagename + "\\" + imagename + "_SAR_Fusion.xml" << endl;
		outfile << "<Opt_ResultTXTPath>" << endl;
		outfile << PluginfoldPath + "\\SAR_Result\\" + imagename + "\\Opt_result.txt" << endl;
		outfile << "<SAR_ResultTXTPath>" << endl;
		outfile << PluginfoldPath + "\\SAR_Result\\" + imagename + "\\SAR_result.txt" << endl;
		outfile << "<Platform>" << endl;
		outfile << "Unknown" << endl;
		outfile << "<MinLongtitude>" << endl;
		outfile << 0 << endl;
		outfile << "<MaxLongitude>" << endl;
		outfile << nImgSizeX << endl;
		outfile << "<MinLatitude>" << endl;
		outfile << -nImgSizeY << endl;
		outfile << "<MaxLatitude>" << endl;
		outfile << 0 << endl;
		outfile << "<PixelSize>" << endl;
		outfile << 1 << endl;
		outfile << "<ImageRows>" << endl;
		outfile << nImgSizeY << endl;
		outfile << "<ImageCols>" << endl;
		outfile << nImgSizeX << endl;

		outfile.close();

		//写ImageINFO.txt
		outfile1 << "<SARImagePath>" << endl;
		outfile1 << imgdes << endl;
		outfile1 << "<RefImagePath>" << endl;
		outfile1 << PluginfoldPath + "\\SAR_Result\\"+ imagename +"\\img_en.tif" << endl;
		outfile1 << "<OutputDir>" << endl;
		outfile1 << PluginfoldPath + "\\SAR_Result\\" + imagename + "\\" << endl;
		outfile1 << "<ImageName>" << endl;
		outfile1 << imagename << endl;
		outfile1 << "<ImageHeight(Rows)>" << endl;
		outfile1 << nImgSizeY << endl;
		outfile1 << "<ImageWidth(Cols)>" << endl;
		outfile1 << nImgSizeX << endl;
		outfile1 << "<LatMin>" << endl;
		outfile1 << -nImgSizeY << endl;
		outfile1 << "<LatMax>" << endl;
		outfile1 << 0 << endl;
		outfile1 << "<LongMin>" << endl;
		outfile1 << 0 << endl;
		outfile1 << "<LongMax>" << endl;
		outfile1 << nImgSizeX << endl;
		outfile1 << "<PixelSize>" << endl;
		outfile1 << 1 << endl;
		outfile1 << "<CoordinateDelta>" << endl;
		outfile1 << 0 << endl;

		outfile1.close();
		return 0;
	}
#pragma endregion
#pragma region 2.2 地理编码中有投影信息
	//地理坐标信息
	double adfGeoTransform[6];
	poDataset->GetGeoTransform(adfGeoTransform);
	for (int i = 0; i < 6; i++)
	{ 
		cout << setiosflags(ios::fixed) << setprecision(12) << adfGeoTransform[i] << endl;
	}

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
	coortransform(preLpszSrcWkt, &adfGeoTransform[0], 4, X, Y, X_lon, Y_lat);

	for (int i = 0; i<4; i++) {
		cout << setiosflags(ios::fixed) << setprecision(13) << "(" << X_lon[i] << " , " << Y_lat[i] << ")" << endl;
	}

	double MaxLon = X_lon[3];
	double MinLon = X_lon[0];
	double MaxLat = Y_lat[0];
	double MinLat = Y_lat[3];

	//写conifg.txt
	//写入config.txt
	outfile << "<SARImagePath>" << endl;
	outfile << imgdes << endl;
	outfile << "<SARImageName>" << endl;
	outfile << imagename << endl;
	outfile << "<MaskPath>" << endl;
	outfile << PluginfoldPath + "\\SAR_Result\\" + imagename + "\\SAR_maskimg.tif" << endl;
	outfile << "<RefImagePath>" << endl;
	outfile << PluginfoldPath + "\\SAR_Result\\" + imagename + "\\img_en.tif" << endl;
	outfile << "<TemplatePath>" << endl;
	outfile << PluginfoldPath + "\\Template\\" << endl;
	outfile << "<ResultImagePath>" << endl;
	outfile << PluginfoldPath + "\\SAR_Result\\" + imagename + "\\" << endl;
	outfile << "<ResultXmlPath>" << endl;
	outfile << PluginfoldPath + "\\SAR_Result\\" + imagename + "\\" + imagename + "_SAR.xml" << endl;
	outfile << "<SARFusionXmlPath>" << endl;
	outfile << PluginfoldPath + "\\SAR_Result\\" + imagename + "\\" + imagename + "_SAR_Fusion.xml" << endl;
	outfile << "<Opt_ResultTXTPath>" << endl;
	outfile << PluginfoldPath + "\\SAR_Result\\" + imagename + "\\Opt_result.txt" << endl;
	outfile << "<SAR_ResultTXTPath>" << endl;
	outfile << PluginfoldPath + "\\SAR_Result\\" + imagename + "\\SAR_result.txt" << endl;
	outfile << "<Platform>" << endl;
	outfile << "Unknown" << endl;
	outfile << "<MinLongtitude>" << endl;
	outfile << setiosflags(ios::fixed) << setprecision(13) << MinLon << endl;
	outfile << "<MaxLongitude>" << endl;
	outfile << MaxLon << endl;
	outfile << "<MinLatitude>" << endl;
	outfile << MinLat << endl;
	outfile << "<MaxLatitude>" << endl;
	outfile << MaxLat << endl;
	outfile << "<PixelSize>" << endl;
	outfile << adfGeoTransform[1] << endl;
	outfile << "<ImageRows>" << endl;
	outfile << nImgSizeY << endl;
	outfile << "<ImageCols>" << endl;
	outfile << nImgSizeX << endl;

	outfile.close();

	//写ImageINFO.txt
	outfile1 << "<SARImagePath>" << endl;
	outfile1 << imgdes << endl;
	outfile1 << "<RefImagePath>" << endl;
	outfile1 << PluginfoldPath + "\\SAR_Result\\" + imagename + "\\img_en.tif" << endl;
	outfile1 << "<OutputDir>" << endl;
	outfile1 << PluginfoldPath + "\\SAR_Result\\" + imagename + "\\" << endl;
	outfile1 << "<ImageName>" << endl;
	outfile1 << imagename << endl;
	outfile1 << "<ImageHeight(Rows)>" << endl;
	outfile1 << nImgSizeY << endl;
	outfile1 << "<ImageWidth(Cols)>" << endl;
	outfile1 << nImgSizeX << endl;
	outfile1 << "<LatMin>" << endl;
	outfile1 << setiosflags(ios::fixed) << setprecision(13) << MinLat << endl;
	outfile1 << "<LatMax>" << endl;
	outfile1 << MaxLat << endl;
	outfile1 << "<LongMin>" << endl;
	outfile1 << MinLon << endl;
	outfile1 << "<LongMax>" << endl;
	outfile1 << MaxLon << endl;
	outfile1 << "<PixelSize>" << endl;
	outfile1 << adfGeoTransform[1] << endl;

	outfile1.close();
#pragma endregion
#pragma endregion


    return 0;
}

