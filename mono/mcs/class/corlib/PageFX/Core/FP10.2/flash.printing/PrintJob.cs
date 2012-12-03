using System;
using System.Runtime.CompilerServices;

namespace flash.printing
{
    /// <summary>
    /// The PrintJob class lets you create content and print it to one or
    /// more pages. This class
    /// lets you render content that is visible, dynamic or off-screen to the user, prompt users with a
    /// single Print dialog box, and print an unscaled document with
    /// proportions that map to the proportions of the content. This
    /// capability is especially useful for rendering and printing dynamic
    /// content, such as database content and dynamic text.
    /// </summary>
    [PageFX.AbcInstance(239)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class PrintJob : flash.events.EventDispatcher
    {
        /// <summary>
        /// The overall paper height, in points. This property is available only
        /// after a call to the PrintJob.start() method has been made.
        /// </summary>
        public extern virtual int paperHeight
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The overall paper width, in points. This property is available only
        /// after a call to the PrintJob.start() method has been made.
        /// </summary>
        public extern virtual int paperWidth
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The height of the actual printable area on the page, in points.
        /// Any user-set margins are ignored. This property is available only
        /// after a call to the PrintJob.start() method has been made.
        /// </summary>
        public extern virtual int pageHeight
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The width of the actual printable area on the page, in points.
        /// Any user-set margins are ignored. This property is available only
        /// after a call to the PrintJob.start() method has been made.
        /// </summary>
        public extern virtual int pageWidth
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The image orientation for printing. This property is a value from the
        /// PrintJobOrientation class. This property is available only after a call to the
        /// PrintJob.start() method has been made.
        /// </summary>
        public extern virtual Avm.String orientation
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static bool isSupported
        {
            [PageFX.AbcClassTrait(8)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern PrintJob();

        /// <summary>
        /// Displays the operating system&apos;s Print dialog box, starts spooling, and sets the PrintJob read-only property values. The Print dialog box lets the user change print settings. When the PrintJob.start() method returns successfully (the user clicks OK in the Print dialog box), the following read-only properties are populated, representing the user&apos;s current print settings:
        /// PropertyTypeUnitsNotesPrintJob.paperHeightNumberPointsOverall paper height.PrintJob.paperWidthNumberPointsOverall paper width.PrintJob.pageHeightNumberPointsHeight of actual printable area on the page; any user-set margins are ignored.PrintJob.pageWidthNumberPointsWidth of actual printable area on the page; any user-set margins are ignored.PrintJob.orientationString&quot;portrait&quot; (flash.printing.PrintJobOrientation.PORTRAIT) or &quot;landscape&quot; (flash.printing.PrintJobOrientation.LANDSCAPE).Note: If the user cancels the Print dialog box, the properties are not populated.After the user clicks OK in the Print dialog box, the player begins spooling a print job to the operating system. Because the operating system then begins displaying information to the user about the printing progress, you should call the PrintJob.addPage() and PrintJob.send() calls as soon as possible to send pages to the spooler. You can use the read-only height, width, and orientation properties this method populates to format the printout.Test to see if this method returns true (when the user clicks OK in the operating system&apos;s Print dialog box) before any subsequent calls to PrintJob.addPage() and PrintJob.send():
        /// var my_pj:PrintJob = new PrintJob();
        /// if(my_pj.start()) {
        /// // addPage() and send() statements here
        /// }
        /// For the given print job instance, if any of the following intervals last more than
        /// 15 seconds the next call to PrintJob.start() will return false:PrintJob.start() and the first PrintJob.addPage()One PrintJob.addPage() and the next PrintJob.addPage()The last PrintJob.addPage() and PrintJob.send()
        /// </summary>
        /// <returns>A value of true if the user clicks OK when the Print dialog box appears; false if the user clicks Cancel or if an error occurs.</returns>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool start();

        /// <summary>
        /// Sends spooled pages to the printer after PrintJob.start() and PrintJob.addPage() have been successful. Calls to PrintJob.send() will not be successful if the call to PrintJob.start() fails, or PrintJob.addpage() throws an exception. So, you should check for PrintJob.start() to return true, and catch any PrintJob.addpage() exceptions before calling PrintJob.send(). For example:
        /// var my_pj:PrintJob = new PrintJob();
        /// if (my_pj.start()) {
        /// try {
        /// my_pj.addPage([params]);
        /// }
        /// catch(e:Error) {
        /// // handle error
        /// }
        /// my_pj.send();
        /// }
        /// </summary>
        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void send();

        /// <summary>
        /// Sends the specified Sprite object as a single page to the print spooler. Before using this
        /// method, you must create a PrintJob object and then use PrintJob.start(). Then,
        /// after calling PrintJob.addPage() one or more times for a print job, you use
        /// PrintJob.send() to send the spooled pages to the printer. In other words, after you create
        /// a PrintJob object, use (in the following sequence) PrintJob.start(),
        /// PrintJob.addPage(), and then PrintJob.send() to send the print job to the
        /// printer. You can use PrintJob.addPage() multiple times after a single call to
        /// PrintJob.start() to print several pages at once.
        /// If PrintJob.addPage() causes Flash Player to throw an exception (for example, if you haven&apos;t called PrintJob.start()
        /// or the user canceled the print job), any subsequent calls to PrintJob.addPage()
        /// will fail. However, if previous calls to PrintJob.addPage() were successful,
        /// the concluding PrintJob.send() command sends the successfully spooled pages to the printer.If the print job takes more than 15 seconds to complete a PrintJob.addPage() operation, Flash Player will throw an exception on the next Print.addPage() call.If you pass a value for the printArea parameter, the x and y coordinates of the
        /// printArea object map to the upper-left corner (0,0 coordinates) of the printable area on the page.
        /// The printable area is described by the read-only pageHeight and pageWidth properties set by
        /// PrintJob.start(). Because the printout aligns with the upper-left corner of the printable area on the
        /// page, when the area defined in printArea is bigger than the printable area on the page, the
        /// printout is cropped to the right or bottom (or both) of the area defined by printArea.
        /// If you don&apos;t pass a value for printArea and the Stage is larger
        /// than the printable area, the same type of clipping occurs.If you want to scale a Sprite object before you print it, set scale
        /// properties (see flash.display.DisplayObject.scaleX and flash.display.DisplayObject.scaleY) before calling this method, and set them back to their original values after printing. The scale of a Sprite object has no relation
        /// to printArea. That is, if you specify a print area that is 50 x 50 pixels, 2500 pixels are printed.
        /// If you scale the Sprite object, the same 2500 pixels are printed, but the Sprite object is printed at the scaled size.The Flash Player printing feature supports PostScript and non-PostScript printers. Non-PostScript printers convert vectors to bitmaps.
        /// </summary>
        /// <param name="sprite">The instance name of the Sprite to print.</param>
        /// <param name="printArea">
        /// (default = null)   A Rectangle object that specifies the area to print.
        /// A rectangle&apos;s width and height are pixel values. A printer uses points as print units of measurement. Points are a fixed physical size (1/72 inch), but the size of a pixel, onscreen, depends on the resolution of the particular screen. So, the conversion rate between pixels and points depends on the printer settings and whether the sprite is scaled. An unscaled sprite that is 72 pixels wide will print out one inch wide, with one point equal to one pixel, independent of screen resolution.You can use the following equivalencies to convert inches
        /// or centimeters to twips or points (a twip is 1/20 of a point):
        /// 1 point = 1/72 inch = 20 twips1 inch = 72 points = 1440 twips1 cm = 567 twipsIf you omit the printArea parameter, or if it is passed incorrectly, the full area of
        /// sprite is printed.If you don&apos;t want to specify a value for printArea but want to specify a value for options
        /// or frameNum, pass null for printArea.
        /// </param>
        /// <param name="options">
        /// (default = null)  An optional parameter that specifies whether to print as vector or bitmap.
        /// The default value is null, which represents a request for vector printing.
        /// To print sprite as a
        /// bitmap, set the printAsBitmap property of the PrintJobOptions object
        /// to true. Remember the following suggestions when determining whether to
        /// set printAsBitmap to true:
        /// If the content that you&apos;re printing includes a bitmap image, set
        /// printAsBitmap to true to include any alpha transparency
        /// and color effects.If the content does not include bitmap images, omit this parameter
        /// to print the content in higher quality vector format.If options is omitted or is passed incorrectly, vector printing is used.
        /// If you don&apos;t want to specify a value for
        /// options but want to specify a value for frameNumber,
        /// pass null for options.
        /// </param>
        /// <param name="frameNum">
        /// (default = 0)  An optional number that lets you specify which frame to print; passing a frameNum
        /// does not cause the ActionScript on that frame to be invoked. If you omit this parameter, the current frame in sprite
        /// is printed.An optional number that is used in the Flash authoring environment. When writing Flex applications,
        /// you should omit this parameter or pass a value of 0.Note: If you previously used print(), printAsBitmap(),
        /// printAsBitmapNum(), or printNum() to print from Flash, you might have used a #p frame label
        /// on multiple frames to specify which pages to print. To use PrintJob.addPage() to print multiple frames, you must use
        /// a PrintJob.addPage() method for each frame; #p frame labels are ignored. For one way to do this
        /// programmatically, see the Example section.
        /// </param>
        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void addPage(flash.display.Sprite sprite, flash.geom.Rectangle printArea, flash.printing.PrintJobOptions options, int frameNum);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addPage(flash.display.Sprite sprite, flash.geom.Rectangle printArea, flash.printing.PrintJobOptions options);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addPage(flash.display.Sprite sprite, flash.geom.Rectangle printArea);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addPage(flash.display.Sprite sprite);


    }
}
