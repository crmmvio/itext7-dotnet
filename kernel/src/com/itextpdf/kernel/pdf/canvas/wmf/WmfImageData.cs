/*
$Id: 05e9aa977b97f99b8ea9993443e6fa555341302e $

This file is part of the iText (R) project.
Copyright (c) 1998-2016 iText Group NV
Authors: Bruno Lowagie, Paulo Soares, et al.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
using System;
using System.IO;
using com.itextpdf.io.image;
using com.itextpdf.io.util;
using com.itextpdf.kernel;

namespace com.itextpdf.kernel.pdf.canvas.wmf
{
	/// <summary>Image implementation for WMF, Windows Metafile.</summary>
	public class WmfImageData : ImageData
	{
		private static readonly byte[] wmf = new byte[] { unchecked((byte)0xD7), unchecked(
			(byte)0xCD) };

		/// <summary>Creates a WmfImage from a file.</summary>
		/// <param name="fileName">pah to the file</param>
		/// <exception cref="java.net.MalformedURLException"/>
		public WmfImageData(String fileName)
			: this(UrlUtil.ToURL(fileName))
		{
		}

		/// <summary>Creates a WmfImage from a URL.</summary>
		/// <param name="url">URL to the file</param>
		public WmfImageData(Uri url)
			: base(url, ImageType.WMF)
		{
			byte[] imageType = ReadImageType(url);
			if (!ImageTypeIs(imageType, wmf))
			{
				throw new PdfException(PdfException.IsNotWmfImage);
			}
		}

		/// <summary>Creates a WmfImage from a byte[].</summary>
		/// <param name="bytes">the image bytes</param>
		public WmfImageData(byte[] bytes)
			: base(bytes, ImageType.WMF)
		{
			byte[] imageType = ReadImageType(url);
			if (!ImageTypeIs(imageType, wmf))
			{
				throw new PdfException(PdfException.IsNotWmfImage);
			}
		}

		private static bool ImageTypeIs(byte[] imageType, byte[] compareWith)
		{
			for (int i = 0; i < compareWith.Length; i++)
			{
				if (imageType[i] != compareWith[i])
				{
					return false;
				}
			}
			return true;
		}

		private static byte[] ReadImageType<T>(T source)
		{
			Stream @is = null;
			try
			{
				if (source is Uri)
				{
					@is = ((Uri)source).OpenStream();
				}
				else
				{
					@is = new MemoryStream((byte[])source);
				}
				byte[] bytes = new byte[8];
				@is.Read(bytes);
				return bytes;
			}
			catch (System.IO.IOException e)
			{
				throw new PdfException(PdfException.IoException, e);
			}
			finally
			{
				if (@is != null)
				{
					try
					{
						@is.Close();
					}
					catch (System.IO.IOException)
					{
					}
				}
			}
		}
	}
}