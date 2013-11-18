#ifndef HEAD_H
#define HEAD_H

#include <opencv2/objdetect/objdetect.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/imgproc/imgproc.hpp>

#include "Eyes.h"

class Head
{
public:
	Head();
	void detectAndDisplay(cv::Mat frame);

private:
	Eyes * eyes;
	cv::Rect faces;
};

#endif /* defined(__EyeTracker__GazeEstimatoea__) */
