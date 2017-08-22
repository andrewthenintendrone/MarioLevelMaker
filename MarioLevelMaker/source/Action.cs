using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioLevelMaker.source
{
    public class Action
    {
        public Action(PixelBox newPixelBox, int newPreviousState, int newCurrentState)
        {
            pixelBox = newPixelBox;
            previousState = newPreviousState;
            currentState = newCurrentState;
        }

        public PixelBox pixelBox;
        public int previousState;
        public int currentState;
    }
}
