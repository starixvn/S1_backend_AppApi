# 2020/10 Mobile API Server

1.JWT token for login `/api/auth`  logout `/api/auth/signout`
2.Saba Sports Token service get token `/api/token/gettoken` ,  invalidate token `/api/token/invalidate` , verify token `/api/token/verifytoken` 

# Mobile Auth API

for APP login and get auth token for later use (access token has 30mins life for now).

## /api/auth

Login with username, password and site Id.

[request parameters](https://www.notion.so/66077f497a574022ad3d0fd43a4bd66e)

[error code](https://www.notion.so/c06bd3fb454a40279fe91794ba8e02f0)

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

## /api/auth/signout

Logout from server.

[request parameters](https://www.notion.so/a95adac2bb6944988ba6466504298f6d)

[request headers](https://www.notion.so/c5df1d2907124817952a4d014cbc32e5)

[error code](https://www.notion.so/5524dfffb7164571a764d6f70405086f)

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

## /api/token/getToken

Get access token form target channel, right now only support Saba Sports so don't need pass parameters.

[request parameters](https://www.notion.so/9d689220ba1d40ffaa17faf011a64dde)

[request headers](https://www.notion.so/00873c59d8ea4b29b2e06a2ca76f31bb)

[error code](https://www.notion.so/ad5a42dcb19d4504b2aafadfe7f5590b)

most of time it will bypass channel's response error message, so didn't list in here.

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

## /api/token/invalidate

disable the user's access token.

[request parameters](https://www.notion.so/3674f4bc984d4bbf9681c97fab602f59)

[request headers](https://www.notion.so/b7a55b7854cb4f4ba7310838c5352e6d)

[error code](https://www.notion.so/19af64a44f634bdcb6ad23d5c19692a7)

most of time it will bypass channel's response error message, so didn't list in here.

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

## /api/token/verifyToken

Verify user token, if success, it will return vendor member id (Saba Sports)

[request parameters](https://www.notion.so/88458f8facb349c392909334010d5b3a)

[request headers](https://www.notion.so/8275bcacfdf9498ca532372e3d1ef28c)

[error code](https://www.notion.so/87a27e3b566e42e7a1dad69f7071587b)

most of time it will bypass channel's response error message, so didn't list in here.

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