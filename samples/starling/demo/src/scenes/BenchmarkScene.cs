namespace demo.scenes
{
	using System;
    using flash.system;
    using starling.core;
    using starling.display;
    using starling.events;
    using starling.text;
    
    public class BenchmarkScene : Scene
    {
        private Button mStartButton;
        private TextField mResultText;
        
        private Sprite mContainer;
        private int mFrameCount;
        private double mElapsed;
        private bool mStarted;
        private int mFailCount;
        private int mWaitFrames;
        
        public BenchmarkScene()
        {
            // the container will hold all test objects
            mContainer = new Sprite();
            mContainer.touchable = false; // we do not need touch events on the test objects -- 
                                          // thus, it is more efficient to disable them.
            addChildAt(mContainer, 0);
            
            mStartButton = new Button(Game.assets.getTexture("button_normal"), "Start benchmark");
            mStartButton.triggered += onStartButtonTriggered;
            mStartButton.x = Constants.CenterX - (mStartButton.width / 2);
            mStartButton.y = 20;
            addChild(mStartButton);
            
            mStarted = false;
            mElapsed = 0.0;
            
            enterFrame += onEnterFrame;
        }
        
        public override void dispose()
        {
            enterFrame -= onEnterFrame;
            mStartButton.triggered -= onStartButtonTriggered;
            base.dispose();
        }
        
        private void onEnterFrame(EnterFrameEvent e)
        {
            if (!mStarted) return;
            
            mElapsed += e.passedTime;
            mFrameCount++;
            
            if (mFrameCount % mWaitFrames == 0)
            {
                var fps = mWaitFrames / mElapsed;
                var targetFps = Starling.current.nativeStage.frameRate;
                
                if (Avm.Math.ceil(fps) >= targetFps)
                {
                    mFailCount = 0;
                    addTestObjects();
                }
                else
                {
                    mFailCount++;
                    
                    if (mFailCount > 20)
                        mWaitFrames = 5; // slow down creation process to be more exact
                    if (mFailCount > 30)
                        mWaitFrames = 10;
                    if (mFailCount == 40)
                        benchmarkComplete(); // target fps not reached for a while
                }
                
                mElapsed = mFrameCount = 0;
            }
            
            var numObjects = mContainer.numChildren;
            var passedTime = e.passedTime;
            
            for (var i = 0; i < numObjects; ++i)
                mContainer.getChildAt(i).rotation += Math.PI / 2 * passedTime;
        }
        
        private void onStartButtonTriggered(Event e)
        {
            // trace("Starting benchmark");
            
            mStartButton.visible = false;
            mStarted = true;
            mFailCount = 0;
            mWaitFrames = 2;
            mFrameCount = 0;
            
            if (mResultText != null)
            {
                mResultText.removeFromParent(true);
                mResultText = null;
            }
            
            addTestObjects();
        }
        
        private void addTestObjects()
        {
            var padding = 15;
            var numObjects = mFailCount > 20 ? 2 : 10;
            
            for (var i = 0; i<numObjects; ++i)
            {
                var egg = new Image(Game.assets.getTexture("benchmark_object"));
                egg.x = padding + Avm.Math.random() * (Constants.GameWidth - 2 * padding);
                egg.y = padding + Avm.Math.random() * (Constants.GameHeight - 2 * padding);
                mContainer.addChild(egg);
            }
        }
        
        private void benchmarkComplete()
        {
            mStarted = false;
            mStartButton.visible = true;
            
            var fps = Starling.current.nativeStage.frameRate;
            
            //trace("Benchmark complete!");
            //trace("FPS: " + fps);
            //trace("Number of objects: " + mContainer.numChildren);

	        var resultString = starling.utils.Global.formatString("Result:\n{0} objects\nwith {1} fps",
		        mContainer.numChildren, fps);
            mResultText = new TextField(240, 200, resultString);
            mResultText.fontSize = 30;
            mResultText.x = Constants.CenterX - mResultText.width / 2;
            mResultText.y = Constants.CenterY - mResultText.height / 2;
            
            addChild(mResultText);
            
            mContainer.removeChildren();
            System.pauseForGCIfCollectionImminent();
        }
    }
}