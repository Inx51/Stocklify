const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = function (app) {
    if(process.env['RUNS_ASPIRE'] !== 'true') {
        return;
    }
    app.use(
        '/signalr',
        createProxyMiddleware({
            target: process.env['services__faker-protocol-gateway__https__0'],
            changeOrigin: false,
            ws: true,
            secure: false,
            pathRewrite: (path, req) => {
                const newPath = path.replace(/^\/signalr/, '');
                return newPath;
            },
        })
    );
};