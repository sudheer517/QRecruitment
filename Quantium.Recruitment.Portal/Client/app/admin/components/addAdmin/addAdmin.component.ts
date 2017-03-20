import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { AdminService } from '../../services/admin.service';
import { DepartmentService } from '../../services/department.service';
import { AdminDto, DepartmentDto } from '../../../RemoteServicesProxy';
import { ModalDirective } from 'ng2-bootstrap/modal';
import { Response } from '@angular/http';


@Component({
    selector: "[appc-add-admin]",
    templateUrl: "./addAdmin.component.html",
    styleUrls: ["./addAdmin.component.scss"],
})
export class AddAdminComponent implements OnInit{

    departments: DepartmentDto[];

    newAdmin: AdminDto;
    hasValidDept = false;
    isEnteredEmailExists= false;
    validationInProgress = false;
    isRequestProcessing = true;
    modalResponse: string;
    @ViewChild('progress') progressModal: ModalDirective;
    constructor(private adminService: AdminService, private departmentService: DepartmentService) {
        
    }
    
    ngOnInit(){
        this.changeBackground();
        this.newAdmin = new AdminDto();
        this.newAdmin.DepartmentId = 0;
        this.departmentService.GetAll().subscribe(
            result => this.departments = result
        );
        
    }

    closeProgressModal(){
        this.progressModal.hide();
    }

    validateEmail(emailInput: FormControl){
        if(emailInput.valid){
            this.validationInProgress = true;
            this.adminService.GetAdminByEmail(emailInput.value).subscribe(
                result => {
                    this.isEnteredEmailExists = true;
                    this.validationInProgress = false;
                    
                },
                error =>{
                    this.validationInProgress = false;
                    this.isEnteredEmailExists = false;
                } 
            )
        }
    }

    addAdmin(modalContent: FormControl){
        this.modalResponse = "Adding admin";
        this.progressModal.show();
        this.adminService.AddAdmin(this.newAdmin).subscribe(
            result => {
                this.modalResponse = "Added admin";
                this.isRequestProcessing = false;
                modalContent.reset();
            },
            (error: Response) => {
                if(error.status === 409){
                    this.modalResponse = "Duplicate admin found";
                }
                else{
                    this.modalResponse = "Add Admin failed";
                }
                
                this.isRequestProcessing = false;
            }
        );
    }

    private changeBackground(){
        let body = document.getElementsByTagName('body')[0];
        body.className.split(' ').forEach(item => item.trim().length > 0 && body.classList.remove(item));
        body.classList.add("jobs-addAdmin-background");
    }
}