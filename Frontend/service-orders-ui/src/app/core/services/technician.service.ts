import { HttpClient } from "@angular/common/http";
import { environment } from "../../../enviroments";
import { Injectable } from "@angular/core";
import { Technician } from "../models/technician.model";

@Injectable({
    providedIn: 'root'
})
export class TechnicianService {

    private api =
        `${environment.apiUrl}/Technicians`;

    constructor(
        private http: HttpClient
    ) {}

    getAll() {
        return this.http.get<Technician[]>(
            this.api
        );
    }

    getById(id:number) {
        return this.http.get<Technician>(
        `${this.api}/${id}`
        );
    }

    create(data:Technician) {
        return this.http.post(
            this.api,
        data
        );
    }

    update(id:number,data:Technician) {
        return this.http.put(
                `${this.api}/${id}`,
            data
            );
        }

    delete(id:number) {
        return this.http.delete(
            `${this.api}/${id}`
        );
    }
}