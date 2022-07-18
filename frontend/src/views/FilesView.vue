<template>
    <section class="content">
        <div class="actions">
            <button
                type="button"
                v-if="parentPath !== null"
                class="actionButton"
                @click="onBackButtonClick"
            >
                <img svg-inline src="../assets/icons/arrow-back.svg" />
            </button>
        </div>

        <div class="listBlock">
            <header class="header">
                <div class="sectionTitles">
                    <h1 class="mainTitle">File Manager</h1>
                    <span class="currentPath">{{ currentPath }}</span>
                </div>

                <div v-if="isCreatingNewFile" class="creationBlock">
                    <input
                        v-bind:value="newFileName"
                        @input="onNewFileNameChange"
                        v-bind:class="[{ isInvalid: !isNewFileNameValid }, 'input']"
                    />
                    <button type="button" @click="submitNewFileName" class="createButton">
                        <span class="buttonText">OK</span>
                    </button>
                </div>
            </header>

            <div class="listTitles">
                <span>Name</span>
                <span>Last modifying</span>
            </div>

            <ul v-if="!isListEmpty" class="list">
                <li v-for="file in files" v-bind:key="file.id" class="listItem">
                    <button
                        type="button"
                        v-bind:class="['card', file.is_directory ? 'isDirectory' : 'isFile']"
                        @click="onCardClick(file)"
                    >
                        <span class="name">{{ file.name }}</span>

                        <span v-if="!file.is_directory" class="lastModifiedAt">{{
                            formatDate(file.last_modified_at)
                        }}</span>
                        <img svg-inline v-if="file.is_directory" src="../assets/icons/folder.svg" />
                    </button>
                    <button type="button" class="deleteButton" @click="onDeleteButtonClick(file)">
                        <img svg-inline src="../assets/icons/trash-cat.svg" />
                    </button>
                </li>
            </ul>
        </div>

        <div class="actions">
            <button
                type="button"
                v-if="!isCreatingNewFile"
                @click="onCreateButtonClick"
                class="actionButton"
            >
                <img svg-inline src="../assets/icons/add.svg" class="addIcon" />
            </button>
        </div>
    </section>
</template>

<script lang="ts">
import { Options, Vue } from "vue-class-component";

import { FileData } from "@/types/file";

import Api from "@/api/Api";

const ERROR_MESSAGE = "Ошибка сервера, возможно у вас нет прав для выполнения данного действия";

export default class FileList extends Vue {
    currentPath: string = "C:\\";
    parentPath: string | null = null;

    files: FileData[] = [];
    newFileName: string | null = null;

    isLoading: boolean = false;
    isNewFileNameValid: boolean = true;
    isCreatingNewFile: boolean = false;
    isListEmpty: boolean = true;

    created() {
        this.fetchFiles();
    }

    fetchFiles() {
        if (this.isLoading) return;

        this.isLoading = true;

        Api.fetchFiles(this.currentPath)
            .then((response) => {
                this.files = response.data.files;
                this.isListEmpty = this.files.length === 0;
                this.parentPath = response.data.parent_path;
            })
            .catch((e) => console.error(ERROR_MESSAGE, e))
            .finally(() => {
                this.isLoading = false;
            });
    }

    formatDate(date: Date) {
        return new Date(date).toLocaleDateString();
    }

    onNewFileNameChange(event: Event) {
        const value = (event.target as HTMLInputElement).value;

        this.newFileName = value;
        this.isNewFileNameValid = value !== null && value !== "";
    }

    onBackButtonClick() {
        if (this.isLoading) return;

        if (this.parentPath !== null) {
            this.currentPath = this.parentPath;
            this.isCreatingNewFile = false;
            this.resetCreation();
            this.fetchFiles();
        }
    }

    onCardClick(file: FileData) {
        if (this.isLoading) return;

        if (file.is_directory) {
            this.currentPath = file.full_name;
            this.isCreatingNewFile = false;
            this.resetCreation();
            this.fetchFiles();
        } else {
            Api.executeFile(file.full_name).catch((e) => console.error(ERROR_MESSAGE, e));
        }
    }

    onDeleteButtonClick(file: FileData) {
        if (this.isLoading) return;

        Api.deleteFile(file.full_name)
            .then(() => this.fetchFiles())
            .catch((e) => console.error(ERROR_MESSAGE, e));
    }

    onCreateButtonClick() {
        this.isCreatingNewFile = true;
        this.resetCreation();
    }

    submitNewFileName() {
        if (this.isNewFileNameValid && this.newFileName !== null) {
            const path = `${this.currentPath}\\${this.newFileName}`;

            Api.createFile(path)
                .then(() => {
                    this.fetchFiles();
                })
                .catch((e) => {
                    console.error(ERROR_MESSAGE, e);
                    this.isNewFileNameValid = false;
                });
        }

        this.isCreatingNewFile = false;
        this.resetCreation();
    }

    resetCreation() {
        this.isNewFileNameValid = true;
        this.newFileName = null;
    }
}
</script>

<style lang="scss" scoped>
@import "../assets/styles/variables";
@import "../assets/styles/utils";

$listPaddingX: 24px;

%button {
    display: flex;
    border: none;
    padding: 0;
    margin: 0;
    align-items: center;
    justify-content: center;
    cursor: pointer;
}

%textWithEllipsis {
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
}

.content {
    position: relative;
    display: grid;
    width: 100%;
    grid-template-columns: 1fr auto 1fr;
    align-content: center;
    justify-content: center;
}

.actions {
    display: flex;
    padding: 144px 28px 0;

    &:first-child {
        justify-content: end;
    }
}

.actionButton {
    @extend %button;

    @include transition(background-color);

    width: 40px;
    height: 40px;
    background-color: rgba($colorGray, 0.5);
    color: white;
    border-radius: 50%;

    &:hover {
        background-color: rgba($colorGray, 0.4);
    }

    &:active,
    &:focus-visible {
        background-color: rgba($colorGray, 0.55);
    }
}

.listBlock {
    width: 750px;
    max-width: 750px;
}

.header {
    display: flex;
    justify-content: space-between;
}

.creationBlock {
    display: flex;
    margin-top: 5px;
}

.input {
    height: 40px;
    outline: none;
    border: 2px solid rgba($colorGray, 0.5);
    border-radius: 20px;
    padding: 0 24px;

    &.isInvalid {
        border-color: $colorRed;
    }
}

.createButton {
    @extend %button;

    @include transition(background-color);

    height: 40px;
    width: 40px;
    background-color: rgba($colorGray, 0.5);
    color: white;
    border-radius: 50%;
    margin-left: 12px;

    &:hover {
        background-color: rgba($colorGray, 0.4);
    }

    &:active,
    &:focus-visible {
        background-color: rgba($colorGray, 0.55);
    }

    .buttonText {
        font-weight: 600;
    }
}

.sectionTitles {
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    padding: 0 $listPaddingX;
    margin-bottom: 44px;
}

.mainTitle {
    white-space: nowrap;
    font-size: toRem(42px);
    color: $colorGray;
    margin: 0 0 4px;
}

.currentPath {
    white-space: wrap;
    font-size: toRem(16px);
    color: rgba($colorGray, 0.5);
}

.listTitles {
    display: flex;
    font-size: toRem(14px);
    color: rgba($colorGray, 0.6);
    padding: 0 $listPaddingX;
    margin-bottom: 12px;
    align-items: center;
    justify-content: space-between;
}

.list {
    list-style-type: none;
    margin: 0;
    padding: 0;
}

.listItem {
    display: flex;
    border-radius: 20px;
    overflow: hidden;

    &:not(:last-child) {
        margin-bottom: 16px;
    }

    &:hover {
        .card {
            min-width: calc(100% - #{52px});
        }

        .deleteButton {
            transform: scale(1);
            margin-left: 12px;
        }
    }
}

.card {
    @extend %button;

    @include transition((min-width, background-color, color), 150ms);

    display: flex;
    max-width: 100%;
    min-width: 100%;
    height: 40px;
    color: $colorGray;
    border-radius: 20px;
    padding: 0 12px 0 $listPaddingX;
    align-items: center;
    justify-content: space-between;

    &:hover {
        color: rgba($colorGray, 0.8);

        &.isDirectory {
            background-color: rgba($colorPurple, 0.3);
        }

        &.isFile {
            background-color: rgba($colorYellow, 0.2);
        }
    }

    &:active,
    &:focus-visible {
        color: $colorGray;

        &.isDirectory {
            background-color: rgba($colorPurple, 0.5);
        }

        &.isFile {
            background-color: rgba($colorYellow, 0.55);
        }
    }

    &.isDirectory {
        background-color: rgba($colorPurple, 0.42);
    }

    &.isFile {
        background-color: rgba($colorYellow, 0.42);
    }
}

.name {
    @extend %textWithEllipsis;

    font-size: toRem(16px);
    font-weight: 600;
}

.lastModifiedAt {
    font-size: toRem(14px);
    font-weight: 400;
    padding-left: 12px;
}

.deleteButton {
    @extend %button;

    @include transition((background-color, transform, margin-left), $timingFunction: linear);

    min-width: 40px;
    height: 40px;
    background-color: rgba($colorRed, 0.5);
    color: white;
    border-radius: 50%;
    margin-left: 0;
    transform: scale(0);

    &:hover {
        background-color: rgba($colorRed, 0.4);
    }

    &:active,
    &:focus-visible {
        background-color: rgba($colorRed, 0.55);
    }
}

.addIcon {
    width: 14px;
    height: 14px;
}
</style>
