import { Mapper } from "./services/mapper.service";

export enum ContentType {
    Category,
    Item
}

export class ContentViewModel {
    constructor(model?: ContentViewModel) {
        if (model) {
            Mapper.map(model, this);
        }
    }

    id: number;
    categoryId?: number;
    title: string;
    imgSrc: string;
    price?: string;
    description?: string;
    contentType: ContentType;
}

export class SiteViewModel {
    constructor(model?: SiteViewModel) {
        if (model) {
            Mapper.map(model, this);
        }
    }

    id: number;
    name: string;
    name2: string;
    homeText: string;
    contactText: string;
    phone1: string;
    phone2: string;
    email: string;
}