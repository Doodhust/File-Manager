/* eslint-disable */
declare module "*.vue" {
    import type { DefineComponent } from "vue";
    const component: DefineComponent<{}, {}, any>;
    export default component;
}

declare module "*.svg" {
    import type { VNode } from "vue";
    type Svg = VNode;
    const component: Svg;
    export default component;
}
