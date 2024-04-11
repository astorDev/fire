const { createHash, randomUUID } = require('crypto');
const jwt = require("jsonwebtoken");

function signJwt(path, bodyJson, privateKey, apiKey) {
    const token = jwt.sign({
        uri: path,
        nonce: randomUUID(),
        iat: Math.floor(Date.now() / 1000),
        exp: Math.floor(Date.now() / 1000) + 55,
        sub: apiKey,
        bodyHash: createHash("sha256").update(JSON.stringify(bodyJson || "")).digest().toString("hex")
    }, privateKey, { algorithm: "RS256"});

    return token;
}

module.exports = {
    configureHooks: (api) => {
        api.hooks.onRequest.addHook('setupAuth', function (request, ctx) {
            var secret = ctx.variables["FIREBLOCKS_API_SECRET"];
            var apiKey = ctx.variables["FIREBLOCKS_API_KEY"];
            var baseUrl = ctx.variables["FIREBLOCKS_URL"];

            if (!request.url.startsWith(baseUrl)) return;

            var bodyJson = JSON.stringify(request["body"]);
            let path = request.url.replace(baseUrl, '');
            var privateKey = `-----BEGIN PRIVATE KEY-----
${secret}
-----END PRIVATE KEY-----`

            var signedJwt = signJwt(path, bodyJson, privateKey, apiKey)

            request.headers = Object.assign({
                'X-API-KEY' : apiKey,
                'Authorization' : `Bearer ${signedJwt}`
            }, request.headers);
        });
    }
};