import pandas as pd
import os
import sys
import json

class ExcelManager:
    def __init__(self, filename):
        self.filename = filename
         # Create a new DataFrame
        self.df = pd.DataFrame()

        # Always overwrite the existing file
        self.df.to_excel(filename, index=False)  # Create or overwrite the Excel file

    def save(self):
        self.df.to_excel(self.filename, index=False)
        print(f"Data saved to {self.filename}")

    def add_header(self, headers):
        for header in headers:
            if header not in self.df.columns:
                self.df[header] = pd.NA
        # print(f"Headers added: {headers}")

    def remove_header(self, headers):
        self.df.drop(columns=[header for header in headers if header in self.df.columns], inplace=True, errors='ignore')
        # print(f"Headers removed: {headers}")

    def add_row(self, row_data):
        new_row = pd.Series(row_data, index=self.df.columns)
        self.df = self.df._append(new_row, ignore_index=True)
        # print(f"Row added: {row_data}")

    def remove_row(self, index):
        if 0 <= index < len(self.df):
            self.df.drop(index=index, inplace=True)
            self.df.reset_index(drop=True, inplace=True)
            # print(f"Row at index {index} removed.")
        else:
            print(f"Index {index} is out of bounds.")

    def add_column(self, column_name, default_value=pd.NA):
        if column_name not in self.df.columns:
            self.df[column_name] = default_value
            # print(f"Column added: {column_name}")

    def remove_column(self, column_name):
        if column_name in self.df.columns:
            self.df.drop(columns=[column_name], inplace=True)
            # print(f"Column removed: {column_name}")

    def display_data(self):
        print(self.df)

def get_complexity(element):
    tag_names = list(map(lambda x: x["name"], element["labels"]))
    print(tag_names)
    if "complex" in tag_names:
        return "complex"
    if "medium" in tag_names:
        return "medium"
    if "simple" in tag_names:
        return "simple"
        
def get_LOC(element):
    tag_names = list(map(lambda x: x["name"], element["labels"]))
    if "simple" in tag_names:
        return 60
    if "medium" in tag_names:
        return 120
    if "complex" in tag_names:
        return 240

# Example usage
if __name__ == "__main__":
    excel_manager = ExcelManager("data.xlsx")

    input_data = sys.stdin.read().strip()
    # print(f"INPUT\n{input_data}")

    # Load JSON data into a Python list
    try:
        data = json.loads(input_data)
    except json.JSONDecodeError as e:
        print(f"Error decoding JSON: {e}")
        sys.exit(1)
       
    # Add headers
    excel_manager.add_header(["ID", "Title", "Status", "Assignee", "Complexity", "Quality", "LOC"])
    # Process each element in the JSON array
    for index,issue in enumerate(data):
        complexity = get_complexity(issue)
        assigns = ', '.join(map(lambda x: x["login"],issue["assignees"]))
        LOC = get_LOC(issue)
        # print(f"================\n\n At element {issue.json()} \n\n complexity {complexity} \n LOC {LOC}")
        result = {
            "ID": index,
            "Title": issue["title"],
            "Status": issue["state"],
            "Assignee": assigns,
            "Complexity": complexity,
            "Quality": "High",
            "LOC": LOC
        }
        excel_manager.add_row(result)
    # Save changes to the Excel file
    excel_manager.save()

