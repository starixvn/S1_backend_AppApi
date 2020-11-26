# 2020/10 Mobile API Server

1. JWT token for login `/api/auth`  logout `/api/auth/signout`  
2. Saba Sports Token service get token `/api/token/gettoken` ,  invalidate token `/api/token/invalidate` , verify token `/api/token/verifytoken`   
3. BillingAPI for transfer `/api/billing/transfer` , balance check `/api/billing/walletInfo` and transfer log `/api/billing/transferRecent`   

# Mobile Auth API

for APP login and get auth token for later use (access token has 30mins life for now).

## /api/auth [POST]

Login with username, password and site Id.

[request headers](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/request%20headers%202a67c6478f744e5daa179540fd57d9a0.csv)

[request parameters](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/request%20parameters%2066077f497a574022ad3d0fd43a4bd66e.csv)

[error code](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/error%20code%20c06bd3fb454a40279fe91794ba8e02f0.csv)

Example

- Request :

    ```json
    {
        "username":"diedtest",
        "password":"12345678",
        "siteId":1
    }
    ```

- Response :
    - Success :

    ```json
    {
        "success": true,
        "code": 0,
        "message": null,
        "data": {
            "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJlZGRpZXRoYiIsImV4cCI6MTYwMzY4MjY0NywianRpIjoiYzE5Mzc2ZTUtMzlmZS00ODgxLTllMTctZjgxZGFhNmJiZjVjIiwiQ3VzdElkIjoiNTY2IiwiVG9rZW4iOiJVSWhrd0F5TTgwS1lmTGxXNUVRYWx3IiwiU2l0ZUlkIjoiMSIsIkxhbmdJZCI6ImVuIiwibmJmIjoxNjAzNjgwODQ3LCJpYXQiOjE2MDM2ODA4NDcsImlzcyI6Ik1vYmlsZUFwaVNlcnZlciJ9.hCbMvoc_TuY1T5qcFPzP710YAHIQVd12rykFqiR699s",
            "tokenType": "bearer",
            "expiresIn": 1799
        }
    }
    ```

    - Fail :

    ```json
    {
        "success": false,
        "code": -4,
        "message": "User or password not match."
    }
    ```

## /api/auth/signout [POST]

Logout from server.

[request parameters](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/request%20parameters%20a95adac2bb6944988ba6466504298f6d.csv)

[request headers](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/request%20headers%20c5df1d2907124817952a4d014cbc32e5.csv)

[error code](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/error%20code%205524dfffb7164571a764d6f70405086f.csv)

Example

- Request :

    ```json
    {}
    ```

- Response :
    - Success :

    ```json
    {
        "success": true,
        "code": 0,
        "message": null,
    }
    ```

    - Fail :

    ```json
    {
        "success": false,
        "code": -3,
        "message": "Operation failed."
    }
    ```

# Token API

## /api/token/getToken [POST]

Get access token form target channel, right now only support Saba Sports so don't need pass parameters.

[request parameters](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/request%20parameters%209d689220ba1d40ffaa17faf011a64dde.csv)

[request headers](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/request%20headers%2000873c59d8ea4b29b2e06a2ca76f31bb.csv)

[error code](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/error%20code%2099b1474fc8764f818db7430d6439b18b.csv)

Example

- Request :

    ```json
    {}
    ```

- Response :

    ```json
    {
        "success": true,
        "code": 0,
        "message": null,
        "data": "ltuyIhPB9xJtfIhEMT4aoyHriS1KZHQbmu"
    }
    ```

## /api/token/invalidate [POST]

disable the user's access token.

[request parameters](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/request%20parameters%203674f4bc984d4bbf9681c97fab602f59.csv)

[request headers](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/request%20headers%20b7a55b7854cb4f4ba7310838c5352e6d.csv)

[error code](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/error%20code%20ab1b5b902634426780a69477c012ffc8.csv)

Example

- Request :

    ```json
    {}
    ```

- Response :

    ```json
    {
        "success": true,
        "code": 0,
        "message": "Success"
    }
    ```

## /api/token/verifyToken [POST]

Verify user token, if success, it will return vendor member id (Saba Sports)

[request parameters](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/request%20parameters%2088458f8facb349c392909334010d5b3a.csv)

[request headers](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/request%20headers%208275bcacfdf9498ca532372e3d1ef28c.csv)

[error code](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/error%20code%20d9b6d221b8ef4a40b0a2610602185da3.csv)

Example

- Request :

    ```json
    {
    		"token":"ltuyIhPB9xJtfIhEMT4aoyHriS1KZHQbmu"
    }
    ```

- Response :

    ```json
    {
        "success": true,
        "code": 0,
        "message": null,
        "data": {
            "vendor_member_id": "DNova88_1152"
        }
    }
    ```

# Billing API

## /api/billing/walletInfo [GET]

Get user wallet info.

[request parameters](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/request%20parameters%205fb251a62d994060b1fe9339f9c44e06.csv)

[request headers](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/request%20headers%20baf22c6c3b764536a707a2735a8700b1.csv)

[error code](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/error%20code%2059a5f61f31da4d12ae2fe3a2a7f1a753.csv)

Example

- Request :

    ```json
    {}
    ```

- Response :

    ```json
    {
        "success": true,
        "code": 0,
        "message": null,
        "data": {
            "balance": 5000.0000,
            "lastLoginTime": "2020-10-29T17:20:00Z",
            "lastTransaction": "2020-10-29T16:45:53Z",
            "isHideBalance": false,
            "tpWallets": [
                {
                    "tpId": 1,
                    "displayKey": "lbl_Wallet_Saba",
                    "tpName": "Sportsbook Wallet",
                    "description": "Include OneRNG, PP and Platinum Suite(AB)",
                    "balance": 66740.0,
                    "bonus": 0.0,
                    "transfer": true,
                    "walletStatus": 1
                }
            ]
        }
    }
    ```

## /api/billing/transferRecent [GET]

Get last 5 user transfer log.

[request parameters](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/request%20parameters%20d2adf14bc81b41568ca7d7fa58f98d3a.csv)

[request headers](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/request%20headers%20de5327199e8e406785a67550572ecb18.csv)

[error code](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/error%20code%20da686d4b4aaf46ecaeba9eaf292d7031.csv)

Example

- Request :

    ```json
    {}
    ```

- Response :

    ```json
    {
        "success": true,
        "code": 0,
        "message": null,
        "data": [
            {
                "tpId": 1,
                "tpName": "Sportsbook Wallet",
                "transId": "TD3440",
                "amount": 2000.00,
                "direction": 2,
                "createTime": "2020-10-29T16:45:52+08:00",
                "status": 0,
                "memo": ""
            },
            {
                "tpId": 1,
                "tpName": "Sportsbook Wallet",
                "transId": "TD3439",
                "amount": 1000.00,
                "direction": 1,
                "createTime": "2020-10-29T16:31:15+08:00",
                "status": 0,
                "memo": ""
            },
            {
                "tpId": 1,
                "tpName": "Sportsbook Wallet",
                "transId": "TD3437",
                "amount": 1.00,
                "direction": 1,
                "createTime": "2020-10-29T15:26:03+08:00",
                "status": 1,
                "memo": "Transfer amount exceeds the limit. ( Amount=0.001, MinTransfer=1.0000, MaxTransfer=2147483647.0000)"
            },
            {
                "tpId": 1,
                "tpName": "Sportsbook Wallet",
                "transId": "TD3436",
                "amount": 1000.00,
                "direction": 1,
                "createTime": "2020-10-29T15:25:44+08:00",
                "status": 0,
                "memo": ""
            },
            {
                "tpId": 1,
                "tpName": "Sportsbook Wallet",
                "transId": "TD3435",
                "amount": 9992000.00,
                "direction": 1,
                "createTime": "2020-10-29T15:23:16+08:00",
                "status": 1,
                "memo": "amount isn't not enough: 9987000.0000"
            }
        ]
    }
    ```

## /api/billing/transfer [POST]

Transfer fund to third party channel.

[request parameters](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/request%20parameters%203f24267a0a534474aab7efa62d5e3602.csv)

[request headers](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/request%20headers%20a38a859103a44169a9503a12322ce3d3.csv)

[error code](2020%2010%20Mobile%20API%20Server%20436cd4b50d514b87b40dc727eca8f8f7/error%20code%2064d4094495ef478cb8c3c43d05f9486b.csv)

Example

- Request :

    ```json
    {
    "tpId": 1, 
    "amount":2000, 
    "direction":2
    }
    ```

- Response :

    ```json
    {
        "success": true,
        "code": 0,
        "message": null,
        "data": {
            "status": 0,
            "thirdId": "2024",
            "difference": 0.0,
            "bonus": 0.0,
            "rollover": 0.0,
            "turnover": 0.0,
            "bonusStatus": 0
        }
    }
    ```