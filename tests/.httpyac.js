const { createHash, randomUUID } = require('crypto');
const jwt = require("jsonwebtoken");

function signJwt(path, bodyJson, privateKey, apiKey) {
    const token = jwt.sign({
        uri: path,
        nonce: randomUUID(),
        iat: Math.floor(Date.now() / 1000),
        exp: Math.floor(Date.now() / 1000) + 55,
        sub: apiKey,
        bodyHash: createHash("sha256").update(bodyJson || "").digest().toString("hex")
    }, privateKey, { algorithm: "RS256"});

    return token;
}

module.exports = {
    configureHooks: (api) => {
        api.hooks.onRequest.addHook('setupAuth', function (request, ctx) {
            var secret = ctx.variables["FIREBLOCKS_API_SECRET"];
            var apiKey = ctx.variables["FIREBLOCKS_API_KEY"];
            var baseUrl = ctx.variables["FIREBLOCKS_URL"];

            if (!request.url.startsWith('https://api.fireblocks.io')) return;

            var bodyJson = request["body"];
            let path = request.url.replace('https://api.fireblocks.io', '');

            var signedJwt = signJwt(path, bodyJson, secret, apiKey)

            request.headers = Object.assign({
                'X-API-KEY' : apiKey,
                //'SECRET' : encodeURIComponent(secret),
                //'X-Body-Json' : bodyJson,
                'Authorization' : `Bearer ${signedJwt}`
            }, request.headers);
        });
    }
};