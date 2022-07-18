const { defineConfig } = require("@vue/cli-service");
module.exports = defineConfig({
    transpileDependencies: true,
    css: {
        loaderOptions: {
            sass: {
                additionalData: `@import "@/assets/styles/variables.scss";`,
            },
        },
    },
    pwa: {
        name: "File Manager",
        themeColor: "#ffffff",
    },
    chainWebpack: (config) => {
        config.module.rule("vue").use("vue-svg-inline-loader").loader("vue-svg-inline-loader");
    },
});
