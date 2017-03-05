import { Component, AfterViewInit, ViewEncapsulation, Renderer, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { AdminService } from '../../services/admin.service';
import { DepartmentService } from '../../services/department.service';
import { AdminDto, DepartmentDto } from '../../../RemoteServicesProxy';
import {NgbModal, ModalDismissReasons, NgbModalRef} from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: "[appc-add-admin]",
    templateUrl: "./addAdmin.component.html",
    styleUrls: ["./addAdmin.component.scss"],
    // encapsulation: ViewEncapsulation.None
})
export class AddAdminComponent implements OnInit{

    departments: DepartmentDto[];

    modalRef: NgbModalRef;
    newAdmin: AdminDto;
    hasValidDept = false;
    isEnteredEmailExists= false;
    validationInProgress = false;
    closeResult: string;
    constructor(private renderer: Renderer, private adminService: AdminService, private departmentService: DepartmentService, private modalService: NgbModal) {
        
    }
    
    open(content) {
        this.modalRef = this.modalService.open(content, { keyboard: false, backdrop: "static", windowClass: "modal-window" });
        
        // this.modalRef.result.then((result) => {
        // this.closeResult = `Closed with: ${result}`;
        // }, (reason) => {
        // this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
        // });
    }
    // private getDismissReason(reason: any): string {
    //     if (reason === ModalDismissReasons.ESC) {
    //     return 'by pressing ESC';
    //     } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
    //     return 'by clicking on a backdrop';
    //     } else {
    //     return  `with: ${reason}`;
    //     }
    // }
    ngOnInit(){
        this.changeBackground();
        this.newAdmin = new AdminDto();
        this.newAdmin.DepartmentId = 0;
        // this.adminService.AddAdmin(new AdminDto(0,"Rohan", "Marthand", "rohan.marthand@hola.com", 9052791243, true, 1)).subscribe(
        //     result => console.log(result), error => console.log(error)
        // )
        this.departmentService.GetAll().subscribe(
            result => this.departments = result
        );
        
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
        this.open(modalContent);
        this.adminService.AddAdmin(this.newAdmin).subscribe(
            result => {
                this.modalRef.close();
            },
            error => {
                this.closeResult = "Add Admin failed";
            }
        );
    }

    private changeBackground(){
        let body = document.getElementsByTagName('body')[0];
        body.className.split(' ').forEach(item => item.trim().length > 0 && body.classList.remove(item));
        body.classList.add("jobs-addAdmin-background");
    }
}