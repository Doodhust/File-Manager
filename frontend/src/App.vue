<template>
    <main class="content">
        <FilesView />
    </main>
</template>

<script lang="ts">
import { Options, Vue } from "vue-class-component";

import FilesView from "@/views/FilesView.vue";

@Options({
    components: {
        FilesView,
    },
})
export default class App extends Vue {}
</script>

<style lang="scss">
body {
    font-family: Avenir, Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    margin: 0;
}

html {
    overflow-x: hidden;
    margin-right: calc(100% - 100vw);
}

* {
    box-sizing: border-box;
}

.content {
    padding: 54px 0;
    color: $colorGray;
}
</style>
