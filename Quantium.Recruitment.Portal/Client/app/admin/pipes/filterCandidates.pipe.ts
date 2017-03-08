import { Pipe, PipeTransform } from '@angular/core';
import { CandidateDto } from '../../RemoteServicesProxy';

@Pipe({
    name: 'filterCandidates'
})
export class FilterCandidatesPipe implements PipeTransform {
    transform(value: CandidateDto[], args: string): any {
    
        if(!args)
            return value;
        let filter = args.toLocaleLowerCase();
        return filter ? 
            value.filter(candidate => 
                candidate.FirstName.toLocaleLowerCase().indexOf(filter) != -1 ||
                candidate.LastName.toLocaleLowerCase().indexOf(filter) != -1 ||
                candidate.Email.toLocaleLowerCase().indexOf(filter) != -1) :
            value;
    }
}