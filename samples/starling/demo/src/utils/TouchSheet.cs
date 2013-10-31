namespace demo.utils
{
	using System;
    
    using starling.display;
    using starling.events;
    
    public class TouchSheet : Sprite
    {
        public TouchSheet(DisplayObject contents)
        {
	        touch += onTouch;
            useHandCursor = true;
            
            if (contents != null)
            {
                contents.x = (int)(contents.width / -2);
                contents.y = (int)(contents.height / -2);
                addChild(contents);
            }
        }
        
        private void onTouch(TouchEvent e)
        {
            var touches = e.getTouches(this, TouchPhase.MOVED);
            
            if (touches.length == 1)
            {
                // one finger touching -> move
                var delta = touches[0].getMovement(parent);
                x += delta.x;
                y += delta.y;
            }            
            else if (touches.length == 2)
            {
                // two fingers touching -> rotate and scale
                var touchA = touches[0];
                var touchB = touches[1];
                
                var currentPosA  = touchA.getLocation(parent);
                var previousPosA = touchA.getPreviousLocation(parent);
                var currentPosB  = touchB.getLocation(parent);
                var previousPosB = touchB.getPreviousLocation(parent);
                
                var currentVector  = currentPosA.subtract(currentPosB);
                var previousVector = previousPosA.subtract(previousPosB);
                
                var currentAngle  = Math.Atan2(currentVector.y, currentVector.x);
                var previousAngle = Math.Atan2(previousVector.y, previousVector.x);
                var deltaAngle = currentAngle - previousAngle;
                
				// update pivot point based on previous center
				var previousLocalA  = touchA.getPreviousLocation(this);
				var previousLocalB  = touchB.getPreviousLocation(this);
				pivotX = (previousLocalA.x + previousLocalB.x) * 0.5;
				pivotY = (previousLocalA.y + previousLocalB.y) * 0.5;
				
				// update location based on the current center
				x = (currentPosA.x + currentPosB.x) * 0.5;
				y = (currentPosA.y + currentPosB.y) * 0.5;
				
				// rotate
                rotation += deltaAngle;

                // scale
                var sizeDiff = currentVector.length / previousVector.length;
                scaleX *= sizeDiff;
                scaleY *= sizeDiff;
            }
            
            var touch = e.getTouch(this, TouchPhase.ENDED);
            
            if (touch != null && touch.tapCount == 2)
                parent.addChild(this); // bring self to front
            
            // enable this code to see when you're hovering over the object
            // touch = e.getTouch(this, TouchPhase.HOVER);            
            // alpha = touch ? 0.8 : 1.0;
        }
        
        public override void dispose()
        {
	        this.touch -= onTouch;
            base.dispose();
        }
    }
}