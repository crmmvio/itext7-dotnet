/*

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
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;

namespace iText.Kernel.Pdf.Annot {
    public class PdfSoundAnnotation : PdfMarkupAnnotation {
        public PdfSoundAnnotation(Rectangle rect, PdfStream sound)
            : base(rect) {
            /*
            There is a problem playing *.wav files via internal player in Acrobat.
            The first byte of the audio stream data should be deleted, then wav file will be played correctly.
            Otherwise it will be broken. Other supporting file types don't have such problem.
            */
            Put(PdfName.Sound, sound);
        }

        public PdfSoundAnnotation(PdfDictionary pdfObject)
            : base(pdfObject) {
        }

        /// <exception cref="System.IO.IOException"/>
        public PdfSoundAnnotation(PdfDocument document, Rectangle rect, Stream soundStream, float sampleRate, PdfName
             encoding, int channels, int sampleSizeInBits)
            : base(rect) {
            PdfStream sound = new PdfStream(document, iText.IO.Util.JavaUtil.CorrectWavFile(soundStream));
            sound.Put(PdfName.R, new PdfNumber(sampleRate));
            sound.Put(PdfName.E, encoding);
            sound.Put(PdfName.B, new PdfNumber(sampleSizeInBits));
            sound.Put(PdfName.C, new PdfNumber(channels));
            Put(PdfName.Sound, sound);
        }

        public override PdfName GetSubtype() {
            return PdfName.Sound;
        }

        public virtual PdfStream GetSound() {
            return GetPdfObject().GetAsStream(PdfName.Sound);
        }
    }
}
