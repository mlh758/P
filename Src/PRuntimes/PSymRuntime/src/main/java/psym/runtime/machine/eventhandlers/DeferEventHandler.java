package psym.runtime.machine.eventhandlers;

import psym.runtime.machine.events.Event;
import psym.runtime.machine.events.Message;
import psym.runtime.machine.Machine;
import psym.valuesummary.Guard;
import psym.valuesummary.PrimitiveVS;
import psym.valuesummary.UnionVS;

public class DeferEventHandler extends EventHandler {

    public DeferEventHandler(Event event) {
        super(event);
    }

    @Override
    public void handleEvent(Guard pc, Machine target, UnionVS payload, EventHandlerReturnReason outcome) {
        Message deferredMessage = new Message(event, new PrimitiveVS<>(), payload);
        target.deferredQueue.defer(pc, deferredMessage.restrict(pc));
    }
}