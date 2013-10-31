using System;
using starling.animation;
using starling.core;
using starling.display;
using starling.events;
using starling.text;
using starling.utils;
using Array = Avm.Array;

namespace demo.scenes
{
    public class AnimationScene : Scene
    {
        private Button mStartButton;
        private Button mDelayButton;
        private Image mEgg;
        private TextField mTransitionLabel;
        private Array mTransitions;
        
        public AnimationScene()
        {
	        mTransitions =
		        new Array(Transitions.LINEAR, Transitions.EASE_IN_OUT,
			        Transitions.EASE_OUT_BACK, Transitions.EASE_OUT_BOUNCE,
			        Transitions.EASE_OUT_ELASTIC);
            
            var buttonTexture = Game.assets.getTexture("button_normal");
            
            // create a button that starts the tween
            mStartButton = new Button(buttonTexture, "Start animation");
            mStartButton.triggered += onStartButtonTriggered;

            mStartButton.x = Constants.CenterX - (int)(mStartButton.width / 2);
            mStartButton.y = 20;
            addChild(mStartButton);
            
            // this button will show you how to call a method with a delay
            mDelayButton = new Button(buttonTexture, "Delayed call");
            mDelayButton.triggered += onDelayButtonTriggered;
            mDelayButton.x = mStartButton.x;
            mDelayButton.y = mStartButton.y + 40;
            addChild(mDelayButton);
            
            // the Starling will be tweened
            mEgg = new Image(Game.assets.getTexture("starling_front"));
            addChild(mEgg);
            resetEgg();
            
            mTransitionLabel = new TextField(320, 30, "", "Verdana", 20, 0, true);
            mTransitionLabel.y = mDelayButton.y + 40;
            mTransitionLabel.alpha = 0.0; // invisible, will be shown later
            addChild(mTransitionLabel);
        }
        
        private void resetEgg()
        {
            mEgg.x = 20;
            mEgg.y = 100;
            mEgg.scaleX = mEgg.scaleY = 1.0;
            mEgg.rotation = 0.0;
        }
        
        private void onStartButtonTriggered(Event e)
        {
            mStartButton.enabled = false;
            resetEgg();
            
            // get next transition style from array and enqueue it at the end
            var transition = mTransitions.shift();
            mTransitions.push(transition);
            
            // to animate any numeric property of an arbitrary object (not just display objects!), 
            // you can create a 'Tween'. One tween object animates one target for a certain time, 
            // a with certain transition function.
            var tween = new Tween(mEgg, 2.0, transition);
            
            // you can animate any property as long as it's numeric (int, uint, Number). 
            // it is animated from it's current value to a target value.  
            tween.animate("rotation", starling.utils.Global.deg2rad(90)); // conventional 'animate' call
            tween.moveTo(300, 360);                 // convenience method for animating 'x' and 'y'
            tween.scaleTo(0.5);                     // convenience method for 'scaleX' and 'scaleY'
            tween.onComplete = (Action)(() => { mStartButton.enabled = true; });
            
            // the tween alone is useless -- for an animation to be carried out, it has to be 
            // advance once in every frame.            
            // This is done by the 'Juggler'. It receives the tween and will carry it out.
            // We use the default juggler here, but you can create your own jugglers, as well.            
            // That way, you can group animations into logical parts.  
            Starling.__juggler.add(tween);
            
            // show which tweening function is used
            mTransitionLabel.text = (string)transition;
            mTransitionLabel.alpha = 1.0;
            
            var hideTween = new Tween(mTransitionLabel, 2.0, Transitions.EASE_IN);
            hideTween.animate("alpha", 0.0);
            Starling.__juggler.add(hideTween);
        }
        
        private void onDelayButtonTriggered(Event e)
        {
            mDelayButton.enabled = false;
            
            // Using the juggler, you can delay a method call. This is especially useful when
            // you use your own juggler in a component of your game, because it gives you perfect 
            // control over the flow of time and animations. 
            
            Starling.__juggler.delayCall((Action)(()=> colorizeEgg(true)), 1.0);
            Starling.__juggler.delayCall((Action)(() => colorizeEgg(false)), 2.0);
            Starling.__juggler.delayCall((Action)(()=> { mDelayButton.enabled = true; }), 2.0);
        }
        
        private void colorizeEgg(bool colorize)
        {
            mEgg.color = colorize ? Color.RED : Color.WHITE;
        }
        
        public override void dispose()
        {
            mStartButton.triggered -= onStartButtonTriggered;
            mDelayButton.triggered -= onDelayButtonTriggered;
            base.dispose();
        }
    }
}