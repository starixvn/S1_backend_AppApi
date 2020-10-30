## Project Overview
- **MobileApiServer**
  - Properties
     - ErrorMessage.en-Us.resx : resource file ready for i18n, but API server seems shouldn't doing i18n.
  - Controllers
     - AuthController.cs : for app get API server auth token
     - BillingController.cs : for app get balance, transfer and get transfer log
     - TestController.cs : some test method seal in #if DEBUG tag , *can be remove if want*
     - TokenController.cs : for app get game token
     - WeatherFrecastController.cs : .NET core demo function , *can be remove if want* 
  - Extensions
     - IndentityExtension.cs : for getting info from user claims
  - Helpers
     - JwtHelpers.cs : handle JWT token stuff 
- **MobileApiServer.Core** => stand alone library for tool and enum
- **MobileApiServer.Handlers** => handle data process before throw to controller layer
  - AccountHandler.cs : login and logout
  - BillingHandler.cs : get balance, transfer and get transfer log
- **MobileApiServer.Models** => some common using model put here
- **MobileApiServer.Providers** => access DB
   - AccountProvider.cs : deal with account stuff
   - BllingProvider.cs : deal with billing stuff
- **MobileApiServer.Services** => access outsite API
   - SabaService.cs : calling Saba Sport mobile API for token (it using interface in case more similar service)
   - TpService.cs : calling self TP server for balance/transfer
  
---  

## TODO  
* add exception/error catch
* add log service (save to file...etc)
* tokenController\GetVendorMemberId can change to get name from DB, but current SP "TpCustInfo_Get" will join 3 table was too wasted
---  

### Develop test account
- for Nova88(siteId=1)  
   diedtest/12345678, qatest01/1q2w3e4r, eddiethb/1q2w3e4r

### Saba Sport Doc
*  https://developer.onebook.dev/MobileAPPIntg/Index?PageName=integration  
   N88_develop/n887730  