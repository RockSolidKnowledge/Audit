# RSK.IdentityServer4.AuditEventSink
Provides Audit Event Sink to add audit records into Admin UI Auditing

# How Does It Work?
To start feeding the events raised from IdentityServer in the Audit process within AdminUI all you need to do is add a reference to the AdminUI Audit Provider and the AdminUI Audit Sink.  You can find these packages here;

[url to nuget packages]
[url to nuget packages]

IdentityServer has a number of events built into it however they are not automatically surfaced to do that you need to enable them;
```csharp
services.AddIdentityServer(options =>
{
    options.Events.RaiseSuccessEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseErrorEvents = true;
});
```
Now you can use the event sink structure within IdentityServer and funnel the events into the AdminUI Audit process.  We do this in the following way;
```csharp
var dbContextOptionsBuilder = new DbContextOptionsBuilder<AuditDatabaseContext>();

RSK.Audit.AuditProviderFactory auditFactory = new AuditProviderFactory(dbContextOptionsBuilder.UseSqlServer(connectionString).Options);

var auditRecorder = auditFactory.CreateAuditSource("IdentityServer");

services.AddSingleton<IEventSink>(provider => new AuditSink(auditRecorder));
```
Lets review what the code actually does.  The initial section of code sets up the AdminUI Audit recorder, this acts as our funnel, and it directs the events into the audit tables used by AdminUI.

The AuditSink acts as our conduit between the two by accepting the events raised by IdentityServer and funnelling them over to the AdminUI Audit process.

# Event Sink Aggregator

One of the small things missing in the event sink process within IdentityServer is ability to have more than one sink.  As we always like to go that little further we have also created an sink aggregator, which you can use in the following way;
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
As you can see from this code our EventSinkAggreator allows you to have more than one event sink in play without one affecting the other.  So, if for any reason, an event sink raises and exception we log that information and ensure all other event sinks in the aggregator still get notified of the IdentityServer events.

# AuditSink Events

When you review the documentation for the events raised by IdentityServer (see here) you will see that there are a number of useful events which will be a great addition to the AdminUI Audit records, however there are a few which don’t fit in.  Due to this, the AuditSink will only handle the following events;

* TokenIssuedSuccessEvent and TokenIssuedFailureEvent
* UserLoginSuccessEvent and UserLoginFailureEvent
* UserLogoutSuccessEvent
* ConsentGrantedEvent and ConsentDeniedEvent
* GrantsRevokedEvent

If you need to ensure that all events are handled, then we have already seen how easy it is to use the EventSinkAggregator
