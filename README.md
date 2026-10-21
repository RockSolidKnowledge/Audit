# IdentityServer Event Sinks for AdminUI

Provides an IdentityServer event sink to add audit records into AdminUI.

- For Duende IdentityServer support, use [Rsk.DuendeIdentityServer.AuditEventSink](https://www.nuget.org/packages/Rsk.DuendeIdentityServer.AuditEventSink/)
- For Open.IdentityServer support, use [Rsk.Open.IdentityServer.AuditEventSink](https://www.nuget.org/packages/Rsk.Open.IdentityServer.AuditEventSink/)

## How Does It Work?

To start feeding the events raised from IdentityServer into the Audit process within AdminUI all you need to do is add a reference to the AdminUI audit provider and the AdminUI audit sink.  You can find these packages here;

- [Rsk.Audit](https://www.nuget.org/packages/RSK.Audit/)
- [Rsk.Audit.Ef](https://www.nuget.org/packages/RSK.Audit.EF/)

IdentityServer has a number of events built into it however they are not automatically surfaced, to do that you need to enable them;

```csharp
services.AddIdentityServer(options =>
{
    options.Events.RaiseSuccessEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseErrorEvents = true;
});
```

Now you can use the event sink structure in IdentityServer to funnel events into the AdminUI audit process. We do this in the following way;

```csharp
var dbContextOptionsBuilder = new DbContextOptionsBuilder<AuditDatabaseContext>();

RSK.Audit.AuditProviderFactory auditFactory = new AuditProviderFactory(dbContextOptionsBuilder.UseSqlServer(connectionString).Options);

var auditRecorder = auditFactory.CreateAuditSource("IdentityServer");

services.AddSingleton<IEventSink>(provider => new AuditSink(auditRecorder));
```

Let's review what the code actually does. The initial section of code sets up the AdminUI audit recorder, which serves as our funnel and directs events to the audit tables used by AdminUI.

The AuditSink acts as our conduit between the two, by accepting the events raised by IdentityServer and funnelling them over to the AdminUI audit process.

## Event Sink Aggregator

One small thing missing in the event sink process within IdentityServer is the ability to use multiple sinks. As we always like to go that little further, we have also created a sink aggregator, which you can use in the following way;

```csharp
services.AddSingleton<IEventSink>(provider => new EventSinkAggregator(_loggerFactory.CreateLogger("EventSinkAggregator"))
  {
      EventSinks = new List<IEventSink>()
      {
          new AuditSink(auditRecorder),
          new MySecondSink()
      }
  });
```

As you can see from this code, our EventSinkAggregator allows you to have more than one event sink in play without one affecting the other. So, if for any reason an event sink raises an exception, we log the exception and ensure that all other event sinks in the aggregator are still notified of IdentityServer events.

## AuditSink Events

When you review the documentation for the events raised by IdentityServer [see here](http://docs.identityserver.io/en/latest/topics/events.html), you will see that there are a number of useful events which are a great addition to the AdminUI Audit records; there are a few which don’t fit in. Due to this, the AuditSink will only handle the following events;

- TokenIssuedSuccessEvent and TokenIssuedFailureEvent
- UserLoginSuccessEvent and UserLoginFailureEvent
- UserLogoutSuccessEvent
- ConsentGrantedEvent and ConsentDeniedEvent
- GrantsRevokedEvent

If you need to ensure that all events are handled, then we have already seen how easy it is to use the EventSinkAggregator
