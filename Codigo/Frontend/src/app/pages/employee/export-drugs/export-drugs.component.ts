import { Component, OnInit } from '@angular/core';
import { cilCheckAlt, cilX } from '@coreui/icons';
import { DrugExportationModel } from 'src/app/interfaces/drug-exportation-model';
import { Parameter } from 'src/app/interfaces/parameter';
import { CommonService } from 'src/app/services/CommonService';
import { ExportService } from 'src/app/services/export.service';

@Component({
  selector: 'app-export-drugs',
  templateUrl: './export-drugs.component.html',
  styleUrls: ['./export-drugs.component.css'],
})
export class ExportDrugsComponent implements OnInit {
  icons = { cilCheckAlt, cilX };
  exporters: string[] = [];
  parameters: Parameter[] = [];
  selectedExporter: any = null;

  constructor(private exportService: ExportService,
    private commonService: CommonService) {}

  ngOnInit(): void {
    this.getExporters();
  }

  getParameters(): void {
    if (this.selectedExporter && this.selectedExporter !== "Select Exporter") {
      this.exportService
        .getParameters(this.selectedExporter)
        .subscribe((params) => (this.parameters = params));
    }else{
      this.selectedExporter = "Select Exporter";
    }
  }

  getExporters(): void {
    this.exportService.getExporters().subscribe((ex) => (this.exporters = ex));
  }

  onSubmit() {
    let drugExportModel: DrugExportationModel;
    if (this.selectedExporter) {
      drugExportModel = {
        formatName: this.selectedExporter,
        parameters: this.parameters,
      };
      this.exportService.export(drugExportModel).subscribe((p: any) => {
        if (p){
          this.commonService.updateToastData(
            `Success Exporting`,
            'success',
            `${this.selectedExporter}`
          );
        }
      });
    }
  }
}
