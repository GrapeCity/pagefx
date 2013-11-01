namespace demo
{
	using Avm;

    public class EmbeddedAssets
    {
        /** ATTENTION: Naming conventions!
         *  
         *  - Classes for embedded IMAGES should have the exact same name as the file,
         *    without extension. This is required so that references from XMLs (atlas, bitmap font)
         *    won't break.
         *    
         *  - Atlas and Font XML files can have an arbitrary name, since they are never
         *    referenced by file name.
         * 
         */
        
        // Texture Atlas
        
        [Embed(Source="textures._1x.atlas.xml", MimeType="application/octet-stream")]
        public static Class atlas_xml;
        
        [Embed(Source="textures._1x.atlas.png")]
        public static Class atlas;

        // Compressed textures
        
        [Embed(Source="textures._1x.compressed_texture.atf", MimeType="application/octet-stream")]
        public static Class compressed_texture;
        
        // Bitmap Fonts
        
        [Embed(Source="fonts._1x.desyrel.fnt", MimeType="application/octet-stream")]
        public static Class desyrel_fnt;
        
        [Embed(Source="fonts._1x.desyrel.png")]
        public static Class desyrel;
        
        // Sounds
        
        [Embed(Source="audio.wing_flap.mp3")]
		public static Class wing_flap;
    }
}