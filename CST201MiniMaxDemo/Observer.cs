using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CST201MiniMaxDemo
{
    public interface Observer
    {
        public void Observe(Board board);
    }
}
