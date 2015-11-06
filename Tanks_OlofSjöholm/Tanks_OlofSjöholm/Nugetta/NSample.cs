using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nugetta
{
    using com.nuggeta;
	using com.nuggeta.api;
	using com.nuggeta.game.core.api.handlers;
	using com.nuggeta.game.core.ngdl.nobjects;
	using com.nuggeta.network.plug;
	using System;
	using System.Collections.Generic;

    public abstract class NSample
    {

        protected NuggetaGamePlug nuggetaPlug;

        protected NuggetaGameApi gameApi;

        protected IOHandler sampleIO;

        private class MyInnerClass : ConnectionInterruptedListener
        {
            private IOHandler sampleIO;

            public MyInnerClass(IOHandler sampleIO) {
                this.sampleIO = sampleIO;
            }

            void ConnectionInterruptedListener.onConnectionInterrupted() {
                sampleIO.log("onConnectionInterrupted");
            }
        }

        public NSample(String url) {
            nuggetaPlug = new NuggetaGamePlug(url);
            nuggetaPlug.setConnectionInterruptedListener(new MyInnerClass(sampleIO));
            nuggetaPlug.setConnectionLostListener(() =>
            {
                sampleIO.log("onConnectionLost");
            });
            gameApi = nuggetaPlug.gameApi();
            gameApi.addNDisconnectedNotificationHandler((NDisconnectedNotification ndisconnectednotification) =>
            {
                sampleIO.log("onNDisconnectedNotification : " + ndisconnectednotification);
            });
        }

        virtual public void onUpdate() {
            nuggetaPlug.pump();
        }

        virtual public void onPaused() {
            if (nuggetaPlug != null) {
            }
        }

        virtual public void onResume() {
            if (nuggetaPlug != null) {
            }
        }

        virtual public void onExit() {
            if (nuggetaPlug != null) {
                nuggetaPlug.stop();
            }
        }

        public abstract void run();

        virtual public void setIo(IOHandler sampleIO) {
            this.sampleIO = sampleIO;
        }

    }
}
