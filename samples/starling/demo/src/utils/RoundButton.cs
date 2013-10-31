namespace demo.utils
{
	using System;
    using flash.geom;    
    using starling.display;
    using starling.textures;
    
    public class RoundButton : Button
    {
        public RoundButton(Texture upState, string text, Texture downState)
			: base(upState, text, downState)
        {
        }
        
        public override DisplayObject hitTest(Point localPoint, bool forTouch)
        {
            // When the user touches the screen, this method is used to find out if an object was 
            // hit. By default, this method uses the bounding box, but by overriding it, 
            // we can change the box (rectangle) to a circle (or whatever necessary).
            
            // when the hit test is done to check if a touch is hitting the object, invisible or
            // untouchable objects must cause the hit test to fail.
            if (forTouch && (!visible || !touchable)) 
                return null; 
            
            // get center of button
            var bounds = this.bounds;
            var centerX = bounds.width / 2;
            var centerY = bounds.height / 2;
            
            // calculate distance of localPoint to center. 
            // we keep it squared, since we want to avoid the 'sqrt()'-call.
	        var sqDist = Math.Pow(localPoint.x - centerX, 2) +
	                     Math.Pow(localPoint.y - centerY, 2);
            
            // when the squared distance is smaller than the squared radius, 
            // the point is inside the circle
            var radius = bounds.width / 2 - 8;
            if (sqDist < Math.Pow(radius, 2)) return this;
	        return null;
        }
    }
}