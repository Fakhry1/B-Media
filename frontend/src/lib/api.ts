const BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? "https://localhost:44344";

export class ApiError extends Error {
  constructor(public status: number, message: string) {
    super(message);
  }
}

function clearAuthToken() {
  if (typeof window === "undefined") return;
  localStorage.removeItem("bmedia_token");
  localStorage.removeItem("bmedia_refresh_token");
  localStorage.removeItem("bmedia_user");
}

export async function apiFetch<T>(
  path: string,
  options: RequestInit = {}
): Promise<T> {
  const token = typeof window !== "undefined" ? localStorage.getItem("bmedia_token") : null;

  const res = await fetch(`${BASE_URL}${path}`, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...options.headers,
    },
  });

  if (!res.ok) {
    const text = await res.text().catch(() => "");
    let message = text;
    try {
      const json = JSON.parse(text);
      message = json.detail ?? json.title ?? json.message ?? text;
    } catch {}

    // Token expired or invalid — clear it so future requests don't keep sending it
    if (res.status === 401 && token) {
      clearAuthToken();
      if (typeof window !== "undefined") window.location.href = "/login";
    }

    throw new ApiError(res.status, message || `HTTP ${res.status}`);
  }

  const text = await res.text();
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  return (text ? JSON.parse(text) : undefined) as T;
}
