using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PegSolitaire
{
    class Move
    {
        public Move(CheckBox checkBox, Direction direction)
        {
            Direction = direction;
            Peg = checkBox;
        }

        public Direction Direction { get; set; }

        public CheckBox Peg { get; set; }
    }
}
