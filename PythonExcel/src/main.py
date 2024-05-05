import os
import requests
import pandas as pd

def download(url, directory, filename):

  path = f"{directory}/{filename}"

  try:
    response = requests.get(url, allow_redirects=True)
    response.raise_for_status()

    with open(path, 'wb') as f:
        f.write(response.content)
        print(f"Downloaded :: {path}")
  
  except requests.exceptions.RequestException as e:
        print(f"Downloading failed :: {path} -> {e}")
   
class StoredExcels:
    def __init__(self, file0_path, file1_path, file0_year, file1_year):
        self.differences = {}
        self.file0       = (pd.read_excel(pd.ExcelFile(file0_path), None), file0_year)
        self.file1       = (pd.read_excel(pd.ExcelFile(file1_path), None), file1_year)
        self.files       = [self.file0[0], self.file1[0]]

    def sort(self, sheet : str, column : str):
        for file in self.files:
            file[sheet][column] = list(
                map("\n".join,
                    map(sorted,
                        map(str.splitlines, file[sheet][column]))))

    def capitalize(self, sheet : str, column : str):
        for file in self.files:
            file[sheet][column] = list(
                map("\n".join,
                    map(lambda n: map(str.capitalize, n),
                        map(str.splitlines, file[sheet][column]))))

    def remove_unnecesary_whitespaces(self):
        for file in self.files:
            for sheet in file.keys():
                for column in file[sheet].keys():
                    file[sheet][column] = list(map(lambda x : str(x).strip(), file[sheet][column]))

    def import_data(self):
        self.sort("Ostrza", "Zastosowanie")

        self.capitalize("Elektronarzedzia", "Nazwa")
        self.capitalize("Elektronarzedzia", "Typ silnika")
        self.capitalize("Elektronarzedzia", "Typ zasilania")
        self.capitalize("Ostrza", "Nazwa")
        self.capitalize("Ostrza", "Typ")
        self.capitalize("Ostrza", "Material")
        self.capitalize("Ostrza", "Zastosowanie")

        self.remove_unnecesary_whitespaces()

    def search_for_matching_id(self, sheet : str, oldRow, file):
        for row in file[sheet].values:
            if(oldRow[0] == row[0]):
                return (True, row) 

        return (False, None)

    def compare_data(self):
        file0 = self.file0[0]
        file1 = self.file1[0]
        
        for sheet in file0.keys():
            added   = []
            deleted = []
            changed = []

            for oldRow in file0[sheet].values:
                id = oldRow[0]
                (found, row) = self.search_for_matching_id(sheet, oldRow, file1)
                if(found):
                    for (index, oldValue) in enumerate(oldRow):
                        column = file0[sheet].keys()[index]
                        value = row[index]
                        if(value != oldValue):
                            changed.append((column, id, oldValue ,value))
                else:
                    deleted.append((id, oldRow))

            for row in file1[sheet].values:
                id = row[0]
                (found, _) = self.search_for_matching_id(sheet, row, file0)
                if(found == False):
                    added.append((id, row))
                    
            self.differences[sheet] = (added, deleted, changed)              

    def get_differences(self):
        changesList = []

        for sheet in self.differences:
            (added, deleted, changed) = self.differences[sheet]

            if(len(added) > 0):
                value = f"Nowe {sheet}: {', '.join(map(lambda x: x[0], added))}"
                changesList.append(value)
                print(value)

            if(len(deleted) > 0):
                value = f"{sheet} wycofane z oferty: {', '.join(map(lambda x: x[0], deleted))}"
                changesList.append(value)
                print(value)

            changed_columns = {}

            for(column, id, _, _) in changed:
                changed_columns.setdefault(column, []).append(id)
            
            for column in changed_columns:
                value = f"Kolumna {column}: zmienila sie dla {sheet}: {', '.join(changed_columns[column])}"
                changesList.append(value)
                print(value)

        return changesList

    def generate_status_sheet(self, sheet):
        (file0, file0_year) = self.file0
        (file1, file1_year) = self.file1
        columns = []
        values  = []
        columns.append("Status")
        columns.append("ID")
        for column in file0[sheet].keys():
            if(column == "ID"):
                continue
            columns.append(f"{column} w {file0_year}")
            columns.append(f"{column} w {file1_year}")

        return pd.DataFrame(values, columns=columns)

    def generate_output_excel(self):
        (file0, file0_year) = self.file0
        (file1, file1_year) = self.file1
        file_path = f"output/raport_zmian_{file0_year}_{file1_year}.xlsx"

        if os.path.exists(file_path):
            os.remove(file_path)

        with pd.ExcelWriter(file_path) as writer:
            pd.DataFrame([f"Raport zmian w latach {file0_year}, {file1_year}"]).to_excel(
                writer,
                sheet_name="Intro",
                header=False,
                index=False)
            pd.DataFrame(self.get_differences(), index=None).to_excel(
                writer,
                sheet_name="Lista zmian",
                header=False,
                index=False)
            self.generate_status_sheet("Elektronarzedzia").to_excel(
                writer,
                sheet_name="Elektronarzedzia",
                index=False)            
            self.generate_status_sheet("Ostrza").to_excel(
                writer,
                sheet_name="Ostrza",
                index=False)


download_directory = "download"
file0_year = "2023"
file1_year = "2024"

file0 = f"input_{file0_year}.xlsx"
file1 = f"input_{file1_year}.xlsx"

download("https://drive.google.com/uc?export=download&id=1wKpSpTx89dbU3SrKt-3-DqQ62kHOFHSX", download_directory, file0)
download("https://drive.google.com/uc?export=download&id=1oYKyW7flL53smo56W9srpFdoUuz6_42x", download_directory, file1)

storedExcels = StoredExcels(f"download/{file0}", f"download/{file1}", file0_year, file1_year)
storedExcels.import_data()
storedExcels.compare_data()
storedExcels.generate_output_excel()