using PushSharp;
using PushSharp.Android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outgoing
{
    public class GCSNotificationSender
    {
        public void Send(List<string> UserIds)
        {
            var push = new PushBroker();

            //Wire up the events for all the services that the broker registers
            push.OnNotificationSent += Push_OnNotificationSent;
            //push.OnChannelException += ChannelException;
            //push.OnServiceException += ServiceException;
            push.OnNotificationFailed += Push_OnNotificationFailed;
            //push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
            //push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
            //push.OnChannelCreated += ChannelCreated;
            //push.OnChannelDestroyed += ChannelDestroyed;


            push.RegisterGcmService(new GcmPushChannelSettings("AIzaSyAA-ukge7o6jO5duM0PS85xBrpCh9MqL-Q"));

            push.QueueNotification(new GcmNotification() { RegistrationIds = UserIds}
                                  .WithJson(@"{""alert"":""Hello World!"",""badge"":7,""sound"":""sound.caf""}"));


            //Stop and wait for the queues to drains before it dispose 
            push.StopAllServices();
        }

        private void Push_OnNotificationFailed(object sender, PushSharp.Core.INotification notification, Exception error)
        {
            throw new NotImplementedException();
        }

        private void Push_OnNotificationSent(object sender, PushSharp.Core.INotification notification)
        {
            throw new NotImplementedException();
        }
    }
}
