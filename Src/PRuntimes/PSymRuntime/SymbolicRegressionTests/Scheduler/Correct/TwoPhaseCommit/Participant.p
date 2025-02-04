machine Participant {
	var coordinator: machine;
    var kvStore: map[int,int];
	var pendingWrTrans: tPrepareForTrans;
	var lastTransId: int;

    start state Init {
	    entry (payload : machine){
		  	coordinator = payload; lastTransId = 0;
			goto WaitForRequests;
		}
	}

	state WaitForRequests {
		on eGlobalAbort do (transId: int) {
			assert (pendingWrTrans.transId == transId);
			if (pendingWrTrans.transId == transId) {
				lastTransId = transId;
			}
		}
		on eGlobalCommit do (transId:int) {
			assert (pendingWrTrans.transId == transId);
			if (pendingWrTrans.transId == transId) {
				kvStore[pendingWrTrans.key] = pendingWrTrans.val;
				announce eMonitor_LocalCommit, (participant = this, transId = transId);
				lastTransId = transId;
			}
		}
		
		on ePrepare do (prepareReq :tPrepareForTrans) { 
			pendingWrTrans = prepareReq;
			assert (pendingWrTrans.transId > lastTransId);
			if ($) {
				send coordinator, ePrepareSuccess, pendingWrTrans.transId;
			} else {
				send coordinator, ePrepareFailed, pendingWrTrans.transId;
			}
		}
	}
}

