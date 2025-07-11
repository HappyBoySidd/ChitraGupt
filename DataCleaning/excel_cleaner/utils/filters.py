import pandas as pd
import numpy as np
import re
from typing import List

def drop_rows_with_exact_hello(df: pd.DataFrame) -> pd.DataFrame:
    mask = df.apply(lambda row: row.astype(str).str.strip().str.lower().eq('hello').any(), axis=1)
    return df[~mask].reset_index(drop=True)

def remove_word_case_insensitive(df: pd.DataFrame, word: str) -> pd.DataFrame:
    pattern = re.compile(rf'\b{re.escape(word)}\b', flags=re.IGNORECASE)
    for col in df.columns:
        df[col] = df[col].map(lambda x: pattern.sub('', str(x)).strip() if pd.notna(x) else x)
    return df

def keep_only_ascii_rows(df: pd.DataFrame) -> pd.DataFrame:
    ascii_mask = df.map(lambda x: all(ord(c) < 128 for c in str(x)) if pd.notna(x) else True)
    return df[ascii_mask.all(axis=1)].reset_index(drop=True)

def drop_rows_with_question_mark(df: pd.DataFrame) -> pd.DataFrame:
    mask = df.astype(str).apply(lambda col: col.str.contains(r'\?', regex=True, na=False))
    return df[~mask.any(axis=1)].reset_index(drop=True)

def normalize_translated_fields(df: pd.DataFrame, cols: List[str]) -> pd.DataFrame:
    for col in cols:
        df[col] = df[col].astype(str).str.strip().replace(r'^\s*$', pd.NA, regex=True)
    df[cols[0]] = df[cols[0]].fillna(df[cols[1]])
    df[cols[1]] = df[cols[1]].fillna(df[cols[0]])
    df = df.dropna(subset=cols, how='all').reset_index(drop=True)
    return df