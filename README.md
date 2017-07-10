# SocialTalents.Sync

Allows to sync multiple servers or webpages over the internet

## Start syncing

http://sync.socialtalents.com/Sync/Register/{syncName}&total={total}&agentId={agentId}
##### syncName
Unique name for your sync, include domain or namespace 
##### total
Number of sync agents to wait for
##### agentId
Current agentId
##### returns
Holds executing till *total* number of unique agents reports, return int plaintext number of agents discovered

### Example
We want to start stress testing agents with lambdas. Since Lambda start can be time-consuming we need to sync them to start testing at the same time.
E.g. we starts 5 lamdas, each of them:
* Generate uniqieId to be used as agentId
* Calls Sync/Register/**loadTestId**&total=**5**&agentId=**uniqueId** 
* When call succeeds, it means all load tests started. 

## Tracking progress

http://sync.socialtalents.com/Sync/Wait/{syncName}&total={total}

##### syncName
Unique name for your sync, include domain or namespace 
##### total
Number of sync agents to wait for
###### Returns number of agents reported
Wait till number of agents changes returns number of agents discovered 

Please note, that actual total number of agents defined by first server call to Register or Wait method and cannot be changed later.

### Feedback
info (at) socialtalents.com