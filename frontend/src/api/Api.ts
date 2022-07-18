import axios, { AxiosResponse } from "axios";
import { FetchFilesResponse } from "@/types/file";

export default class Api {
    private static _get<Response>(pathName: string, param: string, value: string) {
        const backendUrl = "https://localhost:7249";
        const url = new URL(backendUrl);

        url.pathname = pathName;
        url.searchParams.set(param, value);

        return axios.get<Response>(url.href, {
            headers: {
                Accept: "application/json",
            },
        });
    }

    private static _post(pathName: string, param: string, value: string) {
        const backendUrl = "https://localhost:7249";
        const url = new URL(backendUrl);

        url.pathname = pathName;
        url.searchParams.set(param, value);

        return axios.post<never>(url.href);
    }

    public static fetchFiles(path: string): Promise<AxiosResponse<FetchFilesResponse>> {
        return Api._get<FetchFilesResponse>("/api/files/get-files", "path", path);
    }

    public static createFile(path: string): Promise<AxiosResponse<never>> {
        return Api._post("/api/files/create-file", "path", path);
    }

    public static deleteFile(path: string): Promise<AxiosResponse<never>> {
        return Api._post("/api/files/delete-file", "path", path);
    }

    public static executeFile(path: string): Promise<AxiosResponse<never>> {
        return Api._post("/api/files/execute-file", "path", path);
    }
}
