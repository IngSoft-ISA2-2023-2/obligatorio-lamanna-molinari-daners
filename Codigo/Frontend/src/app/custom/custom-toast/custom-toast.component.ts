import { Component, OnInit} from '@angular/core';
import { CommonService } from '../../services/CommonService';

@Component({
  selector: 'app-custom-toast',
  templateUrl: './custom-toast.component.html',
  styleUrls: ['./custom-toast.component.css'],
})
export class CustomToastComponent implements OnInit {
  visible = false;
  message = "";
  color = 'danger';
  title: string = "";

  constructor(
    private commonService: CommonService
  ) {
    var self = this;
    this.commonService.onToastDataUpdate.subscribe((data: any) => {
      if ((Object.keys(data).length !== 0) && data.message !== "") {
        self.visible = true;
        self.message = data.message;
        self.color = data.type;
        self.title = data.title;
      }
    });
  }

  ngOnInit(): void {

  }

  onCloseToast(){
    this.visible = false;
    this.commonService.updateToastData("","","");
  }
}
