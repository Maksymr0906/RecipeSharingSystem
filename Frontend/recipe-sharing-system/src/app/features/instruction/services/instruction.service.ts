import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddInstructionRequest } from '../models/add-instruction-request.model';
import { Observable } from 'rxjs';
import { Instruction } from '../models/instruction.model';
import { environment } from 'src/environments/environment.development';
import { EditInstructionRequest } from '../models/edit-instruction-request.model';

@Injectable({
  providedIn: 'root'
})
export class InstructionService {

  constructor(private http: HttpClient) { }

  addInstruction(model: AddInstructionRequest): Observable<Instruction> {
    return this.http.post<Instruction>(`${environment.apiBaseUrl}/api/instructions`, model);
  }

  getInstructionById(id: string): Observable<Instruction> {
    return this.http.get<Instruction>(`${environment.apiBaseUrl}/api/instructions/${id}`);
  }

  updateInstruction(id: string, model: EditInstructionRequest): Observable<Instruction> {
    return this.http.put<Instruction>(`${environment.apiBaseUrl}/api/instructions/${id}`, model);
  }
}
