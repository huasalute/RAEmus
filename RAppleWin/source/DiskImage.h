#pragma once

/*
AppleWin : An Apple //e emulator for Windows

Copyright (C) 1994-1996, Michael O'Brien
Copyright (C) 1999-2001, Oliver Schmidt
Copyright (C) 2002-2005, Tom Charlesworth
Copyright (C) 2006-2010, Tom Charlesworth, Michael Pohoreski

AppleWin is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

AppleWin is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with AppleWin; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/

#include "DiskDefs.h"
#include <string>

	#define TRACK_DENIBBLIZED_SIZE (16 * 256)	// #Sectors x Sector-size

	#define	TRACKS_STANDARD	35
	#define	TRACKS_EXTRA	5		// Allow up to a 40-track .dsk image (160KB)
	#define	TRACKS_MAX		(TRACKS_STANDARD+TRACKS_EXTRA)

	enum Disk_Status_e
	{
		DISK_STATUS_OFF  ,
		DISK_STATUS_READ ,
		DISK_STATUS_WRITE,
		DISK_STATUS_PROT ,
		NUM_DISK_STATUS
	};

	enum ImageError_e
	{
		eIMAGE_ERROR_NONE,
		eIMAGE_ERROR_BAD_POINTER,
		eIMAGE_ERROR_BAD_SIZE,
		eIMAGE_ERROR_BAD_FILE,
		eIMAGE_ERROR_UNSUPPORTED,
		eIMAGE_ERROR_UNSUPPORTED_HDV,
		eIMAGE_ERROR_GZ,
		eIMAGE_ERROR_ZIP,
		eIMAGE_ERROR_REJECTED_MULTI_ZIP,
		eIMAGE_ERROR_UNSUPPORTED_MULTI_ZIP,
		eIMAGE_ERROR_UNABLE_TO_OPEN,
		eIMAGE_ERROR_UNABLE_TO_OPEN_GZ,
		eIMAGE_ERROR_UNABLE_TO_OPEN_ZIP,
		eIMAGE_ERROR_FAILED_TO_GET_PATHNAME,
		eIMAGE_ERROR_ZEROLENGTH_WRITEPROTECTED,
		eIMAGE_ERROR_FAILED_TO_INIT_ZEROLENGTH,
	};

	const int MAX_DISK_IMAGE_NAME = 15;
	const int MAX_DISK_FULL_NAME  = 127;

struct ImageInfo;

ImageError_e ImageOpen(LPCTSTR* pszImageFilename, ImageInfo** ppImageInfo, bool* pWriteProtected, const bool bCreateIfNecessary, std::string& strFilenameInZip, const bool bExpectFloppy=true);
void ImageClose(ImageInfo* const pImageInfo, const bool bOpenError=false);
BOOL ImageBoot(ImageInfo* const pImageInfo);
void ImageDestroy(void);
void ImageInitialize(void);

void ImageReadTrack(ImageInfo* const pImageInfo, int nTrack, int nQuarterTrack, LPBYTE pTrackImageBuffer, int* pNibbles);
void ImageWriteTrack(ImageInfo* const pImageInfo, int nTrack, int nQuarterTrack, LPBYTE pTrackImage, int nNibbles);
bool ImageReadBlock(ImageInfo* const pImageInfo, UINT nBlock, LPBYTE pBlockBuffer);
bool ImageWriteBlock(ImageInfo* const pImageInfo, UINT nBlock, LPBYTE pBlockBuffer);

int ImageGetNumTracks(ImageInfo* const pImageInfo);
bool ImageIsWriteProtected(ImageInfo* const pImageInfo);
bool ImageIsMultiFileZip(ImageInfo* const pImageInfo);
const char* ImageGetPathname(ImageInfo* const pImageInfo);
UINT ImageGetImageSize(ImageInfo* const pImageInfo);

void GetImageTitle(LPCTSTR pPathname, TCHAR* pImageName, TCHAR* pFullName);
