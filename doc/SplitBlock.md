# Split PDF files by blocks

Split the source PDF files to one PDF file per block that setup in setting file.

## Usage

Usage of `split-block` function.

```
PDFOrganizer: v1.0.0.0

Command 'split-block' - Split the DPF file to blocks by setting

Usage:

split-block --source <String> [--target <String>] --setting <String>

Description:

    --source | --s      The source PDF file
    --target | --t      The target directory, same as source PDF file if not specific
    --config | --j      The config file
```

## Example

To split the PDF file ***merge.pdf*** to ***doc*** folder, and the config file will be ***set.json***, you can:

```
split --s "merge.pdf" --t "doc" --c "set.json"
```

## Split Block Setting

It should be a JSON file that hold a list of block object, such as:

```JSON
[{
    "name": "FileName1",
    "start": 1,
    "end": 8
}, {
    "name": "SomeOther File Name",
    "start": 10,
    "end": 11
}]
```

The properties are:

| Property Name | Description | Validation |
| --- | --- | --- |
| name | The name of the new PDF file that contain this range | Required |
| start | The start page index of the new PDF file that contain this range | Required, 1-totalPage, not larger than `end` |
| end | The end page index  of the new PDF file that contain this range | Required, 1-totalPage, not less than `start`  |
