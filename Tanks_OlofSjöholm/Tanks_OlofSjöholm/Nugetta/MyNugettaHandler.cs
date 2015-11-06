using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nugetta
{
    class MyNugettaHandler
    {

        private static NugettaHandler nugettaHandler;

        private MyNugettaHandler() {

        }

        public static NugettaHandler getInstance(){
            if (nugettaHandler == null) {
                nugettaHandler = new NugettaHandler("127.0.0.1:5010");
            }
            return nugettaHandler;
        }

    }
}
