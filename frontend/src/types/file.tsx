export type FileData = {
    id: number;
    name: string;
    full_name: string;
    last_modified_at: Date;
    is_directory: boolean;
};

export type FetchFilesResponse = {
    files: FileData[];
    parent_path: string | null;
};
